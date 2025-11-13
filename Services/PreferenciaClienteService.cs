using AutoMapper;
using System.Text.Json;
using G2rismBeta.API.DTOs.PreferenciaCliente;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio que contiene toda la lógica de negocio para gestión de Preferencias de Cliente (CRM)
/// </summary>
public class PreferenciaClienteService : IPreferenciaClienteService
{
    private readonly IPreferenciaClienteRepository _preferenciaRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor: Recibe los repositories necesarios y AutoMapper
    /// </summary>
    public PreferenciaClienteService(
        IPreferenciaClienteRepository preferenciaRepository,
        IClienteRepository clienteRepository,
        IMapper mapper)
    {
        _preferenciaRepository = preferenciaRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todas las preferencias del sistema
    /// </summary>
    public async Task<IEnumerable<PreferenciaClienteResponseDto>> GetAllPreferenciasAsync()
    {
        var preferencias = await _preferenciaRepository.GetAllConClientesAsync();
        return _mapper.Map<IEnumerable<PreferenciaClienteResponseDto>>(preferencias);
    }

    /// <summary>
    /// Obtener una preferencia por su ID
    /// </summary>
    public async Task<PreferenciaClienteResponseDto?> GetPreferenciaByIdAsync(int idPreferencia)
    {
        if (idPreferencia <= 0)
        {
            throw new ArgumentException("El ID de la preferencia debe ser mayor a 0", nameof(idPreferencia));
        }

        var preferencia = await _preferenciaRepository.GetPreferenciaConClienteAsync(idPreferencia);

        if (preferencia == null)
        {
            return null;
        }

        return _mapper.Map<PreferenciaClienteResponseDto>(preferencia);
    }

    /// <summary>
    /// Obtener preferencia por ID de cliente (relación 1:1)
    /// </summary>
    public async Task<PreferenciaClienteResponseDto?> GetPreferenciaByClienteIdAsync(int idCliente)
    {
        if (idCliente <= 0)
        {
            throw new ArgumentException("El ID del cliente debe ser mayor a 0", nameof(idCliente));
        }

        var preferencia = await _preferenciaRepository.GetByClienteIdAsync(idCliente);

        if (preferencia == null)
        {
            return null;
        }

        return _mapper.Map<PreferenciaClienteResponseDto>(preferencia);
    }

    /// <summary>
    /// Crear nuevas preferencias para un cliente
    /// </summary>
    public async Task<PreferenciaClienteResponseDto> CreatePreferenciaAsync(PreferenciaClienteCreateDto preferenciaCreateDto)
    {
        // 1. Validaciones de negocio
        await ValidarCrearPreferencia(preferenciaCreateDto);

        // 2. Convertir DTO a entidad
        var preferencia = _mapper.Map<PreferenciaCliente>(preferenciaCreateDto);

        // 3. Convertir lista de intereses a JSON
        if (preferenciaCreateDto.Intereses != null && preferenciaCreateDto.Intereses.Any())
        {
            preferencia.Intereses = JsonSerializer.Serialize(preferenciaCreateDto.Intereses);
        }

        // 4. Establecer fecha de actualización
        preferencia.FechaActualizacion = DateTime.Now;

        // 5. Crear en la base de datos
        var preferenciaCreada = await _preferenciaRepository.AddAsync(preferencia);
        await _preferenciaRepository.SaveChangesAsync();

        // 6. Obtener la preferencia con sus relaciones para retornar
        var preferenciaCompleta = await _preferenciaRepository.GetPreferenciaConClienteAsync(preferenciaCreada.IdPreferencia);

        // 7. Convertir a DTO y retornar
        return _mapper.Map<PreferenciaClienteResponseDto>(preferenciaCompleta);
    }

    /// <summary>
    /// Actualizar preferencias existentes
    /// </summary>
    public async Task<PreferenciaClienteResponseDto> UpdatePreferenciaAsync(int idPreferencia, PreferenciaClienteUpdateDto preferenciaUpdateDto)
    {
        // 1. Validar que existe la preferencia
        var preferenciaExistente = await _preferenciaRepository.GetByIdAsync(idPreferencia);
        if (preferenciaExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró la preferencia con ID {idPreferencia}");
        }

        // 2. Actualizar campos
        _mapper.Map(preferenciaUpdateDto, preferenciaExistente);

        // 3. Convertir lista de intereses a JSON
        if (preferenciaUpdateDto.Intereses != null)
        {
            preferenciaExistente.Intereses = JsonSerializer.Serialize(preferenciaUpdateDto.Intereses);
        }

        // 4. Actualizar fecha
        preferenciaExistente.FechaActualizacion = DateTime.Now;

        // 5. Actualizar en la base de datos
        await _preferenciaRepository.UpdateAsync(preferenciaExistente);
        await _preferenciaRepository.SaveChangesAsync();

        // 6. Obtener la preferencia actualizada con sus relaciones
        var preferenciaActualizada = await _preferenciaRepository.GetPreferenciaConClienteAsync(idPreferencia);

        // 7. Convertir a DTO y retornar
        return _mapper.Map<PreferenciaClienteResponseDto>(preferenciaActualizada);
    }

    /// <summary>
    /// Eliminar preferencias de un cliente
    /// </summary>
    public async Task<bool> DeletePreferenciaAsync(int idPreferencia)
    {
        // 1. Validar que existe la preferencia
        var preferencia = await _preferenciaRepository.GetByIdAsync(idPreferencia);
        if (preferencia == null)
        {
            throw new KeyNotFoundException($"No se encontró la preferencia con ID {idPreferencia}");
        }

        // 2. Eliminar de la base de datos
        await _preferenciaRepository.DeleteAsync(idPreferencia);
        await _preferenciaRepository.SaveChangesAsync();

        return true;
    }

    // ========================================
    // OPERACIONES DE BÚSQUEDA (CRM)
    // ========================================

    /// <summary>
    /// Buscar preferencias por tipo de destino
    /// </summary>
    public async Task<IEnumerable<PreferenciaClienteResponseDto>> GetPreferenciasByTipoDestinoAsync(string tipoDestino)
    {
        if (string.IsNullOrWhiteSpace(tipoDestino))
        {
            throw new ArgumentException("El tipo de destino no puede estar vacío", nameof(tipoDestino));
        }

        var preferencias = await _preferenciaRepository.GetByTipoDestinoAsync(tipoDestino);
        return _mapper.Map<IEnumerable<PreferenciaClienteResponseDto>>(preferencias);
    }

    /// <summary>
    /// Buscar preferencias por rango de presupuesto
    /// </summary>
    public async Task<IEnumerable<PreferenciaClienteResponseDto>> GetPreferenciasByRangoPresupuestoAsync(decimal min, decimal max)
    {
        if (min < 0)
        {
            throw new ArgumentException("El presupuesto mínimo debe ser mayor o igual a 0", nameof(min));
        }

        if (max < min)
        {
            throw new ArgumentException("El presupuesto máximo debe ser mayor o igual al mínimo", nameof(max));
        }

        var preferencias = await _preferenciaRepository.GetByRangoPresupuestoAsync(min, max);
        return _mapper.Map<IEnumerable<PreferenciaClienteResponseDto>>(preferencias);
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si un cliente ya tiene preferencias registradas
    /// </summary>
    public async Task<bool> ClienteTienePreferenciasAsync(int idCliente)
    {
        return await _preferenciaRepository.ClienteTienePreferenciasAsync(idCliente);
    }

    // ========================================
    // MÉTODOS PRIVADOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Validaciones de negocio al crear preferencias
    /// </summary>
    private async Task ValidarCrearPreferencia(PreferenciaClienteCreateDto dto)
    {
        // 1. Validar que el cliente existe
        var cliente = await _clienteRepository.GetByIdAsync(dto.IdCliente);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {dto.IdCliente}");
        }

        // 2. Validar que el cliente no tenga ya preferencias (relación 1:1)
        var tienePreferencias = await _preferenciaRepository.ClienteTienePreferenciasAsync(dto.IdCliente);
        if (tienePreferencias)
        {
            throw new InvalidOperationException($"El cliente con ID {dto.IdCliente} ya tiene preferencias registradas. Use actualizar en su lugar.");
        }

        // 3. Validar presupuesto si se especifica
        if (dto.PresupuestoPromedio.HasValue && dto.PresupuestoPromedio.Value < 0)
        {
            throw new ArgumentException("El presupuesto promedio debe ser un valor positivo");
        }
    }
}