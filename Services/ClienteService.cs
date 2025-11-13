using AutoMapper;
using G2rismBeta.API.DTOs.Cliente;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio que contiene toda la lógica de negocio para gestión de Clientes (CRM)
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ICategoriaClienteRepository _categoriaClienteRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor: Recibe los repositories necesarios y AutoMapper
    /// </summary>
    public ClienteService(
        IClienteRepository clienteRepository,
        ICategoriaClienteRepository categoriaClienteRepository,
        IUsuarioRepository usuarioRepository,
        IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _categoriaClienteRepository = categoriaClienteRepository;
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todos los clientes del sistema
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> GetAllClientesAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    /// <summary>
    /// Obtener solo clientes activos
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> GetClientesActivosAsync()
    {
        var clientes = await _clienteRepository.GetClientesActivosAsync();
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    /// <summary>
    /// Obtener un cliente por su ID
    /// </summary>
    public async Task<ClienteResponseDto?> GetClienteByIdAsync(int idCliente)
    {
        // 1. Validar que el ID sea positivo
        if (idCliente <= 0)
        {
            throw new ArgumentException("El ID del cliente debe ser mayor a 0", nameof(idCliente));
        }

        // 2. Buscar el cliente con su categoría
        var cliente = await _clienteRepository.GetClienteConCategoriaAsync(idCliente);

        // 3. Si no existe, retornar null
        if (cliente == null)
        {
            return null;
        }

        // 4. Convertir a DTO
        return _mapper.Map<ClienteResponseDto>(cliente);
    }

    /// <summary>
    /// Obtener cliente con información detallada de categoría
    /// </summary>
    public async Task<ClienteConCategoriaDto?> GetClienteConCategoriaAsync(int idCliente)
    {
        if (idCliente <= 0)
        {
            throw new ArgumentException("El ID del cliente debe ser mayor a 0", nameof(idCliente));
        }

        var cliente = await _clienteRepository.GetClienteCompletoAsync(idCliente);

        if (cliente == null)
        {
            return null;
        }

        return _mapper.Map<ClienteConCategoriaDto>(cliente);
    }

    /// <summary>
    /// Crear un nuevo cliente
    /// </summary>
    public async Task<ClienteResponseDto> CreateClienteAsync(ClienteCreateDto clienteCreateDto)
    {
        // 1. Validaciones de negocio
        await ValidarCrearCliente(clienteCreateDto);

        // 2. Convertir DTO a entidad
        var cliente = _mapper.Map<Cliente>(clienteCreateDto);

        // 3. Establecer valores por defecto
        cliente.FechaRegistro = DateTime.Now;
        cliente.Estado = true;

        // 4. Crear en la base de datos
        var clienteCreado = await _clienteRepository.AddAsync(cliente);
        await _clienteRepository.SaveChangesAsync();

        // 5. Obtener el cliente con sus relaciones para retornar
        var clienteCompleto = await _clienteRepository.GetClienteConCategoriaAsync(clienteCreado.IdCliente);

        // 6. Convertir a DTO y retornar
        return _mapper.Map<ClienteResponseDto>(clienteCompleto);
    }

    /// <summary>
    /// Actualizar un cliente existente
    /// </summary>
    public async Task<ClienteResponseDto> UpdateClienteAsync(int idCliente, ClienteUpdateDto clienteUpdateDto)
    {
        // 1. Validar que existe el cliente
        var clienteExistente = await _clienteRepository.GetByIdAsync(idCliente);
        if (clienteExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {idCliente}");
        }

        // 2. Validaciones de negocio
        await ValidarActualizarCliente(idCliente, clienteUpdateDto);

        // 3. Actualizar solo los campos permitidos
        _mapper.Map(clienteUpdateDto, clienteExistente);

        // 4. Actualizar en la base de datos
        await _clienteRepository.UpdateAsync(clienteExistente);
        await _clienteRepository.SaveChangesAsync();

        // 5. Obtener el cliente actualizado con sus relaciones
        var clienteActualizado = await _clienteRepository.GetClienteConCategoriaAsync(idCliente);

        // 6. Convertir a DTO y retornar
        return _mapper.Map<ClienteResponseDto>(clienteActualizado);
    }

    /// <summary>
    /// Eliminar un cliente (solo si no tiene reservas asociadas)
    /// </summary>
    public async Task<bool> DeleteClienteAsync(int idCliente)
    {
        // 1. Validar que existe el cliente
        var cliente = await _clienteRepository.GetByIdAsync(idCliente);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {idCliente}");
        }

        // 2. TODO: Verificar que no tenga reservas asociadas (cuando implementemos módulo Reservas)
        // Por ahora, permitimos la eliminación

        // 3. Eliminar de la base de datos
        await _clienteRepository.DeleteAsync(idCliente);
        await _clienteRepository.SaveChangesAsync();

        return true;
    }

    // ========================================
    // OPERACIONES DE BÚSQUEDA
    // ========================================

    /// <summary>
    /// Buscar cliente por documento de identidad
    /// </summary>
    public async Task<ClienteResponseDto?> GetClienteByDocumentoAsync(string documentoIdentidad)
    {
        if (string.IsNullOrWhiteSpace(documentoIdentidad))
        {
            throw new ArgumentException("El documento de identidad no puede estar vacío", nameof(documentoIdentidad));
        }

        var cliente = await _clienteRepository.GetByDocumentoAsync(documentoIdentidad);

        if (cliente == null)
        {
            return null;
        }

        return _mapper.Map<ClienteResponseDto>(cliente);
    }

    /// <summary>
    /// Buscar cliente por ID de usuario
    /// </summary>
    public async Task<ClienteResponseDto?> GetClienteByUsuarioIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(idUsuario));
        }

        var cliente = await _clienteRepository.GetByUsuarioIdAsync(idUsuario);

        if (cliente == null)
        {
            return null;
        }

        return _mapper.Map<ClienteResponseDto>(cliente);
    }

    /// <summary>
    /// Buscar clientes por nombre o apellido
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> BuscarClientesPorNombreAsync(string termino)
    {
        if (string.IsNullOrWhiteSpace(termino))
        {
            throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(termino));
        }

        var clientes = await _clienteRepository.BuscarPorNombreAsync(termino);
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    /// <summary>
    /// Obtener clientes por categoría
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> GetClientesPorCategoriaAsync(int idCategoria)
    {
        // Validar que la categoría existe
        var categoria = await _categoriaClienteRepository.GetByIdAsync(idCategoria);
        if (categoria == null)
        {
            throw new KeyNotFoundException($"No se encontró la categoría con ID {idCategoria}");
        }

        var clientes = await _clienteRepository.GetClientesPorCategoriaAsync(idCategoria);
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    /// <summary>
    /// Obtener clientes por ciudad
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> GetClientesPorCiudadAsync(string ciudad)
    {
        if (string.IsNullOrWhiteSpace(ciudad))
        {
            throw new ArgumentException("La ciudad no puede estar vacía", nameof(ciudad));
        }

        var clientes = await _clienteRepository.GetClientesPorCiudadAsync(ciudad);
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    /// <summary>
    /// Obtener clientes por país
    /// </summary>
    public async Task<IEnumerable<ClienteResponseDto>> GetClientesPorPaisAsync(string pais)
    {
        if (string.IsNullOrWhiteSpace(pais))
        {
            throw new ArgumentException("El país no puede estar vacío", nameof(pais));
        }

        var clientes = await _clienteRepository.GetClientesPorPaisAsync(pais);
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    /// <summary>
    /// Cambiar el estado de un cliente (activar/desactivar)
    /// </summary>
    public async Task<bool> CambiarEstadoClienteAsync(int idCliente, bool nuevoEstado)
    {
        var cliente = await _clienteRepository.GetByIdAsync(idCliente);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {idCliente}");
        }

        var resultado = await _clienteRepository.CambiarEstadoAsync(idCliente, nuevoEstado);
        if (resultado)
        {
            await _clienteRepository.SaveChangesAsync();
        }

        return resultado;
    }

    /// <summary>
    /// Asignar o cambiar la categoría de un cliente
    /// </summary>
    public async Task<bool> AsignarCategoriaAsync(int idCliente, int idCategoria)
    {
        // 1. Validar que existe el cliente
        var cliente = await _clienteRepository.GetByIdAsync(idCliente);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No se encontró el cliente con ID {idCliente}");
        }

        // 2. Validar que existe la categoría
        var categoria = await _categoriaClienteRepository.GetByIdAsync(idCategoria);
        if (categoria == null)
        {
            throw new KeyNotFoundException($"No se encontró la categoría con ID {idCategoria}");
        }

        // 3. Validar que la categoría esté activa
        if (!categoria.Estado)
        {
            throw new InvalidOperationException("No se puede asignar una categoría inactiva");
        }

        // 4. Asignar la categoría
        cliente.IdCategoria = idCategoria;
        await _clienteRepository.UpdateAsync(cliente);
        await _clienteRepository.SaveChangesAsync();

        return true;
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un documento de identidad ya existe
    /// </summary>
    public async Task<bool> DocumentoExisteAsync(string documentoIdentidad, int? idClienteExcluir = null)
    {
        return await _clienteRepository.ExisteDocumentoAsync(documentoIdentidad, idClienteExcluir);
    }

    /// <summary>
    /// Validar si un usuario ya tiene un cliente asociado
    /// </summary>
    public async Task<bool> UsuarioTieneClienteAsync(int idUsuario, int? idClienteExcluir = null)
    {
        return await _clienteRepository.UsuarioTieneClienteAsync(idUsuario, idClienteExcluir);
    }

    // ========================================
    // MÉTODOS PRIVADOS DE VALIDACIÓN
    // ========================================

    /// <summary>
    /// Validaciones de negocio al crear un cliente
    /// </summary>
    private async Task ValidarCrearCliente(ClienteCreateDto dto)
    {
        // 1. Validar que el usuario existe
        var usuario = await _usuarioRepository.GetByIdAsync(dto.IdUsuario);
        if (usuario == null)
        {
            throw new KeyNotFoundException($"No se encontró el usuario con ID {dto.IdUsuario}");
        }

        // 2. Validar que el usuario no tenga ya un cliente asociado
        var usuarioTieneCliente = await _clienteRepository.UsuarioTieneClienteAsync(dto.IdUsuario);
        if (usuarioTieneCliente)
        {
            throw new InvalidOperationException($"El usuario con ID {dto.IdUsuario} ya tiene un cliente asociado");
        }

        // 3. Validar que el documento de identidad no exista
        var documentoExiste = await _clienteRepository.ExisteDocumentoAsync(dto.DocumentoIdentidad);
        if (documentoExiste)
        {
            throw new InvalidOperationException($"Ya existe un cliente con el documento {dto.DocumentoIdentidad}");
        }

        // 4. Si se especifica una categoría, validar que existe y está activa
        if (dto.IdCategoria.HasValue)
        {
            var categoria = await _categoriaClienteRepository.GetByIdAsync(dto.IdCategoria.Value);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"No se encontró la categoría con ID {dto.IdCategoria.Value}");
            }

            if (!categoria.Estado)
            {
                throw new InvalidOperationException("No se puede asignar una categoría inactiva");
            }
        }

        // 5. Validar que la fecha de nacimiento sea válida (mayor de edad)
        var edad = DateTime.Today.Year - dto.FechaNacimiento.Year;
        if (dto.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

        if (edad < 18)
        {
            throw new InvalidOperationException("El cliente debe ser mayor de 18 años");
        }
    }

    /// <summary>
    /// Validaciones de negocio al actualizar un cliente
    /// </summary>
    private async Task ValidarActualizarCliente(int idCliente, ClienteUpdateDto dto)
    {
        // 1. Validar que el documento de identidad no exista (excluyendo el cliente actual)
        var documentoExiste = await _clienteRepository.ExisteDocumentoAsync(dto.DocumentoIdentidad, idCliente);
        if (documentoExiste)
        {
            throw new InvalidOperationException($"Ya existe otro cliente con el documento {dto.DocumentoIdentidad}");
        }

        // 2. Si se especifica una categoría, validar que existe y está activa
        if (dto.IdCategoria.HasValue)
        {
            var categoria = await _categoriaClienteRepository.GetByIdAsync(dto.IdCategoria.Value);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"No se encontró la categoría con ID {dto.IdCategoria.Value}");
            }

            if (!categoria.Estado)
            {
                throw new InvalidOperationException("No se puede asignar una categoría inactiva");
            }
        }

        // 3. Validar que la fecha de nacimiento sea válida (mayor de edad)
        var edad = DateTime.Today.Year - dto.FechaNacimiento.Year;
        if (dto.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

        if (edad < 18)
        {
            throw new InvalidOperationException("El cliente debe ser mayor de 18 años");
        }
    }
}
