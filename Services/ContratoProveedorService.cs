using AutoMapper;
using G2rismBeta.API.DTOs.ContratoProveedor;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio de Contratos de Proveedor
/// Contiene toda la lógica de negocio para la gestión de contratos
/// </summary>
public class ContratoProveedorService : IContratoProveedorService
{
    private readonly IContratoProveedorRepository _contratoRepository;
    private readonly IProveedorRepository _proveedorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ContratoProveedorService> _logger;

    public ContratoProveedorService(
        IContratoProveedorRepository contratoRepository,
        IProveedorRepository proveedorRepository,
        IMapper mapper,
        ILogger<ContratoProveedorService> logger)
    {
        _contratoRepository = contratoRepository;
        _proveedorRepository = proveedorRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // ========================================
    // OPERACIONES CRUD
    // ========================================

    /// <summary>
    /// Obtener todos los contratos
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetAllAsync()
    {
        var contratos = await _contratoRepository.GetAllAsync();
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener un contrato por ID
    /// </summary>
    public async Task<ContratoProveedorResponseDto?> GetByIdAsync(int id)
    {
        var contrato = await _contratoRepository.GetByIdAsync(id);
        if (contrato == null)
            return null;

        return MapToResponseDto(contrato);
    }

    /// <summary>
    /// Crear un nuevo contrato
    /// </summary>
    public async Task<ContratoProveedorResponseDto> CreateAsync(ContratoProveedorCreateDto dto)
    {
        // Validación 1: Verificar que el proveedor existe
        var proveedor = await _proveedorRepository.GetByIdAsync(dto.IdProveedor);
        if (proveedor == null)
        {
            throw new KeyNotFoundException($"Proveedor con ID {dto.IdProveedor} no encontrado");
        }

        // Validación 2: Verificar que el proveedor esté activo
        if (proveedor.Estado != "Activo")
        {
            throw new InvalidOperationException($"No se puede crear un contrato con el proveedor '{proveedor.NombreEmpresa}' porque su estado es '{proveedor.Estado}'. El proveedor debe estar Activo.");
        }

        // Validación 3: Número de contrato único
        if (await _contratoRepository.ExisteNumeroContratoAsync(dto.NumeroContrato))
        {
            throw new InvalidOperationException($"Ya existe un contrato con el número {dto.NumeroContrato}");
        }

        // Validación 4: Fechas válidas
        if (dto.FechaFin <= dto.FechaInicio)
        {
            throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");
        }

        // Validación 5: Estado válido (normalizado)
        var estadoNormalizado = ValidarYNormalizarEstado(dto.Estado);
        dto.Estado = estadoNormalizado;

        // Validación 6: Valor del contrato positivo
        if (dto.ValorContrato < 0)
        {
            throw new ArgumentException("El valor del contrato debe ser mayor o igual a 0");
        }

        // Mapear y crear
        var contrato = _mapper.Map<ContratoProveedor>(dto);
        contrato.FechaCreacion = DateTime.Now;

        var contratoCreado = await _contratoRepository.AddAsync(contrato);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Contrato creado exitosamente: {contratoCreado.NumeroContrato} (ID: {contratoCreado.IdContrato})");

        return MapToResponseDto(contratoCreado);
    }

    /// <summary>
    /// Actualizar un contrato existente
    /// </summary>
    public async Task<ContratoProveedorResponseDto> UpdateAsync(int id, ContratoProveedorUpdateDto dto)
    {
        var contratoExistente = await _contratoRepository.GetByIdAsync(id);
        if (contratoExistente == null)
        {
            throw new KeyNotFoundException($"Contrato con ID {id} no encontrado");
        }

        // Validación 1: Si se actualiza el número de contrato, verificar que sea único
        if (!string.IsNullOrWhiteSpace(dto.NumeroContrato) && dto.NumeroContrato != contratoExistente.NumeroContrato)
        {
            if (await _contratoRepository.ExisteNumeroContratoAsync(dto.NumeroContrato, id))
            {
                throw new InvalidOperationException($"Ya existe otro contrato con el número {dto.NumeroContrato}");
            }
            contratoExistente.NumeroContrato = dto.NumeroContrato;
        }

        // Validación 2: Fechas válidas
        DateTime? nuevaFechaInicio = dto.FechaInicio ?? contratoExistente.FechaInicio;
        DateTime? nuevaFechaFin = dto.FechaFin ?? contratoExistente.FechaFin;

        if (nuevaFechaFin <= nuevaFechaInicio)
        {
            throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");
        }

        // Validación 3: Estado válido (normalizado)
        if (!string.IsNullOrWhiteSpace(dto.Estado))
        {
            var estadoNormalizado = ValidarYNormalizarEstado(dto.Estado);
            contratoExistente.Estado = estadoNormalizado;
        }

        // Validación 4: Valor del contrato
        if (dto.ValorContrato.HasValue)
        {
            if (dto.ValorContrato.Value < 0)
            {
                throw new ArgumentException("El valor del contrato debe ser mayor o igual a 0");
            }
            contratoExistente.ValorContrato = dto.ValorContrato.Value;
        }

        // Actualizar solo los campos que vienen en el DTO
        if (dto.FechaInicio.HasValue)
            contratoExistente.FechaInicio = dto.FechaInicio.Value;

        if (dto.FechaFin.HasValue)
            contratoExistente.FechaFin = dto.FechaFin.Value;

        if (!string.IsNullOrWhiteSpace(dto.TipoContrato))
            contratoExistente.TipoContrato = dto.TipoContrato;

        if (!string.IsNullOrWhiteSpace(dto.CondicionesPago))
            contratoExistente.CondicionesPago = dto.CondicionesPago;

        if (!string.IsNullOrWhiteSpace(dto.Terminos))
            contratoExistente.Terminos = dto.Terminos;

        if (dto.RenovacionAutomatica.HasValue)
            contratoExistente.RenovacionAutomatica = dto.RenovacionAutomatica.Value;

        if (dto.ArchivoContrato != null)
            contratoExistente.ArchivoContrato = dto.ArchivoContrato;

        if (dto.Observaciones != null)
            contratoExistente.Observaciones = dto.Observaciones;

        await _contratoRepository.UpdateAsync(contratoExistente);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Contrato actualizado exitosamente: ID {id}");

        return MapToResponseDto(contratoExistente);
    }

    /// <summary>
    /// Eliminar un contrato
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var contrato = await _contratoRepository.GetByIdAsync(id);
        if (contrato == null)
        {
            throw new KeyNotFoundException($"Contrato con ID {id} no encontrado");
        }

        // Validación: Solo se pueden eliminar contratos en estado 'Cancelado' o 'En_Negociacion'
        if (contrato.Estado == "Vigente")
        {
            throw new InvalidOperationException(
                "No se puede eliminar un contrato vigente. Primero debe cancelarlo.");
        }

        await _contratoRepository.DeleteAsync(id);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Contrato eliminado exitosamente: ID {id}");

        return true;
    }

    // ========================================
    // BÚSQUEDAS Y FILTROS
    // ========================================

    /// <summary>
    /// Buscar contrato por número de contrato
    /// </summary>
    public async Task<ContratoProveedorResponseDto?> GetByNumeroContratoAsync(string numeroContrato)
    {
        var contrato = await _contratoRepository.GetByNumeroContratoAsync(numeroContrato);
        if (contrato == null)
            return null;

        return MapToResponseDto(contrato);
    }

    /// <summary>
    /// Obtener contratos de un proveedor específico
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetByProveedorAsync(int idProveedor)
    {
        var contratos = await _contratoRepository.GetByProveedorAsync(idProveedor);
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener contratos por estado
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetByEstadoAsync(string estado)
    {
        var estadoNormalizado = ValidarYNormalizarEstado(estado);
        var contratos = await _contratoRepository.GetByEstadoAsync(estadoNormalizado);
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener contratos vigentes
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetVigentesAsync()
    {
        var contratos = await _contratoRepository.GetVigentesAsync();
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener contratos próximos a vencer
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetProximosAVencerAsync(int diasAnticipacion = 30)
    {
        if (diasAnticipacion <= 0 || diasAnticipacion > 365)
        {
            throw new ArgumentException("Los días de anticipación deben estar entre 1 y 365");
        }

        var contratos = await _contratoRepository.GetProximosAVencerAsync(diasAnticipacion);
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener contratos vencidos
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetVencidosAsync()
    {
        var contratos = await _contratoRepository.GetVencidosAsync();
        return contratos.Select(MapToResponseDto);
    }

    /// <summary>
    /// Obtener contratos con renovación automática
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetConRenovacionAutomaticaAsync()
    {
        var contratos = await _contratoRepository.GetConRenovacionAutomaticaAsync();
        return contratos.Select(MapToResponseDto);
    }

    // ========================================
    // GESTIÓN DE ESTADO
    // ========================================

    /// <summary>
    /// Cambiar estado de un contrato
    /// </summary>
    public async Task<ContratoProveedorResponseDto> CambiarEstadoAsync(int id, string nuevoEstado)
    {
        var contrato = await _contratoRepository.GetByIdAsync(id);
        if (contrato == null)
        {
            throw new KeyNotFoundException($"Contrato con ID {id} no encontrado");
        }

        var estadoNormalizado = ValidarYNormalizarEstado(nuevoEstado);

        // Validación: No permitir cambiar a Vigente si las fechas no lo permiten
        if (estadoNormalizado == "Vigente")
        {
            var hoy = DateTime.Now.Date;
            if (contrato.FechaInicio.Date > hoy || contrato.FechaFin.Date < hoy)
            {
                throw new InvalidOperationException(
                    $"No se puede cambiar el estado a Vigente. El contrato tiene vigencia desde {contrato.FechaInicio:yyyy-MM-dd} hasta {contrato.FechaFin:yyyy-MM-dd}");
            }
        }

        contrato.Estado = estadoNormalizado;
        await _contratoRepository.UpdateAsync(contrato);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Estado del contrato ID {id} cambiado a: {estadoNormalizado}");

        return MapToResponseDto(contrato);
    }

    /// <summary>
    /// Renovar un contrato (extender fecha de fin)
    /// </summary>
    public async Task<ContratoProveedorResponseDto> RenovarContratoAsync(int id, DateTime nuevaFechaFin)
    {
        var contrato = await _contratoRepository.GetByIdAsync(id);
        if (contrato == null)
        {
            throw new KeyNotFoundException($"Contrato con ID {id} no encontrado");
        }

        // Validación 1: Solo se pueden renovar contratos vigentes o próximos a vencer
        if (contrato.Estado != "Vigente")
        {
            throw new InvalidOperationException("Solo se pueden renovar contratos vigentes");
        }

        // Validación 2: La nueva fecha debe ser posterior a la fecha de fin actual
        if (nuevaFechaFin <= contrato.FechaFin)
        {
            throw new ArgumentException("La nueva fecha de fin debe ser posterior a la fecha de fin actual");
        }

        contrato.FechaFin = nuevaFechaFin;
        await _contratoRepository.UpdateAsync(contrato);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Contrato ID {id} renovado hasta {nuevaFechaFin:yyyy-MM-dd}");

        return MapToResponseDto(contrato);
    }

    /// <summary>
    /// Cancelar un contrato
    /// </summary>
    public async Task<ContratoProveedorResponseDto> CancelarContratoAsync(int id, string motivo)
    {
        var contrato = await _contratoRepository.GetByIdAsync(id);
        if (contrato == null)
        {
            throw new KeyNotFoundException($"Contrato con ID {id} no encontrado");
        }

        // Validación: No se pueden cancelar contratos ya cancelados
        if (contrato.Estado == "Cancelado")
        {
            throw new InvalidOperationException("El contrato ya está cancelado");
        }

        contrato.Estado = "Cancelado";
        contrato.Observaciones = string.IsNullOrWhiteSpace(contrato.Observaciones)
            ? $"Cancelado: {motivo}"
            : $"{contrato.Observaciones}\nCancelado: {motivo}";

        await _contratoRepository.UpdateAsync(contrato);
        await _contratoRepository.SaveChangesAsync();

        _logger.LogInformation($"Contrato ID {id} cancelado. Motivo: {motivo}");

        return MapToResponseDto(contrato);
    }

    // ========================================
    // ESTADÍSTICAS Y REPORTES
    // ========================================

    /// <summary>
    /// Obtener estadísticas de contratos por estado
    /// </summary>
    public async Task<Dictionary<string, int>> GetEstadisticasPorEstadoAsync()
    {
        return await _contratoRepository.CountByEstadoAsync();
    }

    /// <summary>
    /// Obtener valor total de contratos vigentes
    /// </summary>
    public async Task<decimal> GetValorTotalContratosVigentesAsync()
    {
        return await _contratoRepository.GetValorTotalContratosVigentesAsync();
    }

    /// <summary>
    /// Obtener contratos de un proveedor en estado específico
    /// </summary>
    public async Task<IEnumerable<ContratoProveedorResponseDto>> GetByProveedorYEstadoAsync(int idProveedor, string estado)
    {
        var estadoNormalizado = ValidarYNormalizarEstado(estado);
        var contratos = await _contratoRepository.GetByProveedorYEstadoAsync(idProveedor, estadoNormalizado);
        return contratos.Select(MapToResponseDto);
    }

    // ========================================
    // MÉTODOS AUXILIARES
    // ========================================

    /// <summary>
    /// Validar y normalizar el estado del contrato (case-insensitive)
    /// </summary>
    private string ValidarYNormalizarEstado(string estado)
    {
        var estadosValidos = new[] { "Vigente", "Vencido", "Cancelado", "En_Negociacion" };

        // Buscar coincidencia case-insensitive
        var estadoNormalizado = estadosValidos.FirstOrDefault(e =>
            string.Equals(e, estado, StringComparison.OrdinalIgnoreCase));

        if (estadoNormalizado == null)
        {
            throw new ArgumentException($"Estado '{estado}' no es válido. Valores permitidos: {string.Join(", ", estadosValidos)}");
        }

        return estadoNormalizado;
    }

    /// <summary>
    /// Mapear ContratoProveedor a ContratoProveedorResponseDto incluyendo propiedades calculadas
    /// </summary>
    private ContratoProveedorResponseDto MapToResponseDto(ContratoProveedor contrato)
    {
        var dto = _mapper.Map<ContratoProveedorResponseDto>(contrato);

        // Agregar propiedades calculadas
        dto.EstaVigente = contrato.EstaVigente;
        dto.DiasRestantes = contrato.DiasRestantes;
        dto.ProximoAVencer = contrato.ProximoAVencer;
        dto.DuracionDias = contrato.DuracionDias;

        // Agregar nombre del proveedor si está disponible
        if (contrato.Proveedor != null)
        {
            dto.NombreProveedor = contrato.Proveedor.NombreEmpresa;
        }

        return dto;
    }
}
