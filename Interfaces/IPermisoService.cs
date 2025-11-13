using G2rismBeta.API.DTOs.Permiso;

namespace G2rismBeta.API.Interfaces;

/// <summary>
/// Interface del servicio de Permisos
/// </summary>
public interface IPermisoService
{
    // CRUD Básico
    Task<IEnumerable<PermisoResponseDto>> GetAllPermisosAsync();
    Task<IEnumerable<PermisoResponseDto>> GetPermisosActivosAsync();
    Task<PermisoResponseDto?> GetPermisoByIdAsync(int idPermiso);
    Task<IEnumerable<PermisoResponseDto>> GetPermisosPorModuloAsync(string modulo);
    Task<PermisoResponseDto> CreatePermisoAsync(PermisoCreateDto permisoCreateDto);
    Task<PermisoResponseDto> UpdatePermisoAsync(int idPermiso, PermisoUpdateDto permisoUpdateDto);
    Task<bool> DeletePermisoAsync(int idPermiso);

    // Operaciones especiales
    Task<bool> CambiarEstadoPermisoAsync(int idPermiso, bool nuevoEstado);
    Task<IEnumerable<string>> GetModulosAsync();

    // Validaciones
    Task<bool> NombrePermisoExisteAsync(string nombrePermiso, int? idPermisoExcluir = null);
}
