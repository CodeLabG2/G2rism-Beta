using G2rismBeta.API.DTOs.Cliente;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Clientes
/// Define la lógica de negocio para gestión de clientes
/// </summary>
public interface IClienteService
{
    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todos los clientes
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> GetAllClientesAsync();

    /// <summary>
    /// Obtener solo clientes activos
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> GetClientesActivosAsync();

    /// <summary>
    /// Obtener un cliente por su ID
    /// </summary>
    Task<ClienteResponseDto?> GetClienteByIdAsync(int idCliente);

    /// <summary>
    /// Obtener cliente con información detallada de categoría
    /// </summary>
    Task<ClienteConCategoriaDto?> GetClienteConCategoriaAsync(int idCliente);

    /// <summary>
    /// Crear un nuevo cliente
    /// </summary>
    Task<ClienteResponseDto> CreateClienteAsync(ClienteCreateDto clienteCreateDto);

    /// <summary>
    /// Actualizar un cliente existente
    /// </summary>
    Task<ClienteResponseDto> UpdateClienteAsync(int idCliente, ClienteUpdateDto clienteUpdateDto);

    /// <summary>
    /// Eliminar un cliente (solo si no tiene reservas)
    /// </summary>
    Task<bool> DeleteClienteAsync(int idCliente);

    // ========================================
    // OPERACIONES DE BÚSQUEDA
    // ========================================

    /// <summary>
    /// Buscar cliente por documento de identidad
    /// </summary>
    Task<ClienteResponseDto?> GetClienteByDocumentoAsync(string documentoIdentidad);

    /// <summary>
    /// Buscar cliente por ID de usuario
    /// </summary>
    Task<ClienteResponseDto?> GetClienteByUsuarioIdAsync(int idUsuario);

    /// <summary>
    /// Buscar clientes por nombre o apellido
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> BuscarClientesPorNombreAsync(string termino);

    /// <summary>
    /// Obtener clientes por categoría
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> GetClientesPorCategoriaAsync(int idCategoria);

    /// <summary>
    /// Obtener clientes por ciudad
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> GetClientesPorCiudadAsync(string ciudad);

    /// <summary>
    /// Obtener clientes por país
    /// </summary>
    Task<IEnumerable<ClienteResponseDto>> GetClientesPorPaisAsync(string pais);

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    /// <summary>
    /// Cambiar el estado de un cliente (activar/desactivar)
    /// </summary>
    Task<bool> CambiarEstadoClienteAsync(int idCliente, bool nuevoEstado);

    /// <summary>
    /// Asignar o cambiar la categoría de un cliente
    /// </summary>
    Task<bool> AsignarCategoriaAsync(int idCliente, int idCategoria);

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un documento de identidad ya existe
    /// </summary>
    Task<bool> DocumentoExisteAsync(string documentoIdentidad, int? idClienteExcluir = null);

    /// <summary>
    /// Validar si un usuario ya tiene un cliente asociado
    /// </summary>
    Task<bool> UsuarioTieneClienteAsync(int idUsuario, int? idClienteExcluir = null);
}