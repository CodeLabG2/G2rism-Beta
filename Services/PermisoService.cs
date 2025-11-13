using AutoMapper;
using G2rismBeta.API.DTOs.Permiso;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio con lógica de negocio para Permisos
/// </summary>
public class PermisoService : IPermisoService
{
    private readonly IPermisoRepository _permisoRepository;
    private readonly IRolPermisoRepository _rolPermisoRepository;
    private readonly IMapper _mapper;

    public PermisoService(
        IPermisoRepository permisoRepository,
        IRolPermisoRepository rolPermisoRepository,
        IMapper mapper)
    {
        _permisoRepository = permisoRepository;
        _rolPermisoRepository = rolPermisoRepository;
        _mapper = mapper;
    }

    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    public async Task<IEnumerable<PermisoResponseDto>> GetAllPermisosAsync()
    {
        var permisos = await _permisoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PermisoResponseDto>>(permisos);
    }

    public async Task<IEnumerable<PermisoResponseDto>> GetPermisosActivosAsync()
    {
        var permisos = await _permisoRepository.GetPermisosActivosAsync();
        return _mapper.Map<IEnumerable<PermisoResponseDto>>(permisos);
    }

    public async Task<PermisoResponseDto?> GetPermisoByIdAsync(int idPermiso)
    {
        if (idPermiso <= 0)
        {
            throw new ArgumentException("El ID del permiso debe ser mayor a 0", nameof(idPermiso));
        }

        var permiso = await _permisoRepository.GetByIdAsync(idPermiso);

        if (permiso == null)
        {
            return null;
        }

        return _mapper.Map<PermisoResponseDto>(permiso);
    }

    public async Task<IEnumerable<PermisoResponseDto>> GetPermisosPorModuloAsync(string modulo)
    {
        if (string.IsNullOrWhiteSpace(modulo))
        {
            throw new ArgumentException("El nombre del módulo es obligatorio", nameof(modulo));
        }

        var permisos = await _permisoRepository.GetByModuloAsync(modulo);
        return _mapper.Map<IEnumerable<PermisoResponseDto>>(permisos);
    }

    public async Task<PermisoResponseDto> CreatePermisoAsync(PermisoCreateDto permisoCreateDto)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        // 1. Validar campos obligatorios
        if (string.IsNullOrWhiteSpace(permisoCreateDto.Modulo))
        {
            throw new ArgumentException("El módulo es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(permisoCreateDto.Accion))
        {
            throw new ArgumentException("La acción es obligatoria");
        }

        // 2. Generar nombre del permiso automáticamente
        var nombrePermiso = $"{permisoCreateDto.Modulo.ToLower()}.{permisoCreateDto.Accion.ToLower()}";
        permisoCreateDto.NombrePermiso = nombrePermiso;

        // 3. Validar que no exista
        var nombreExiste = await _permisoRepository.ExisteNombrePermisoAsync(nombrePermiso);
        if (nombreExiste)
        {
            throw new InvalidOperationException($"Ya existe el permiso '{nombrePermiso}'");
        }

        // ========================================
        // CREAR PERMISO
        // ========================================

        var nuevoPermiso = _mapper.Map<Permiso>(permisoCreateDto);
        nuevoPermiso.Estado = true;

        var permisoCreado = await _permisoRepository.AddAsync(nuevoPermiso);
        await _permisoRepository.SaveChangesAsync();

        return _mapper.Map<PermisoResponseDto>(permisoCreado);
    }

    public async Task<PermisoResponseDto> UpdatePermisoAsync(int idPermiso, PermisoUpdateDto permisoUpdateDto)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        var permisoExistente = await _permisoRepository.GetByIdAsync(idPermiso);
        if (permisoExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró el permiso con ID {idPermiso}");
        }

        if (string.IsNullOrWhiteSpace(permisoUpdateDto.Modulo))
        {
            throw new ArgumentException("El módulo es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(permisoUpdateDto.Accion))
        {
            throw new ArgumentException("La acción es obligatoria");
        }

        // Regenerar nombre del permiso
        var nuevoNombrePermiso = $"{permisoUpdateDto.Modulo.ToLower()}.{permisoUpdateDto.Accion.ToLower()}";

        // Validar que el nuevo nombre no exista (excluyendo el actual)
        var nombreExiste = await _permisoRepository.ExisteNombrePermisoAsync(nuevoNombrePermiso, idPermiso);
        if (nombreExiste)
        {
            throw new InvalidOperationException($"Ya existe otro permiso con el nombre '{nuevoNombrePermiso}'");
        }

        // ========================================
        // ACTUALIZAR
        // ========================================

        permisoExistente.Modulo = permisoUpdateDto.Modulo;
        permisoExistente.Accion = permisoUpdateDto.Accion;
        permisoExistente.NombrePermiso = nuevoNombrePermiso;
        permisoExistente.Descripcion = permisoUpdateDto.Descripcion;

        await _permisoRepository.UpdateAsync(permisoExistente);
        await _permisoRepository.SaveChangesAsync();

        return _mapper.Map<PermisoResponseDto>(permisoExistente);
    }

    public async Task<bool> DeletePermisoAsync(int idPermiso)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        var permiso = await _permisoRepository.GetByIdAsync(idPermiso);
        if (permiso == null)
        {
            throw new KeyNotFoundException($"No se encontró el permiso con ID {idPermiso}");
        }

        // Validar que no esté asignado a ningún rol
        var roles = await _rolPermisoRepository.GetRolesPorPermisoAsync(idPermiso);
        if (roles.Any())
        {
            throw new InvalidOperationException(
                $"No se puede eliminar el permiso porque está asignado a {roles.Count()} rol(es)");
        }

        // ========================================
        // ELIMINAR
        // ========================================

        var resultado = await _permisoRepository.DeleteAsync(idPermiso);
        await _permisoRepository.SaveChangesAsync();

        return resultado;
    }

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    public async Task<bool> CambiarEstadoPermisoAsync(int idPermiso, bool nuevoEstado)
    {
        var permiso = await _permisoRepository.GetByIdAsync(idPermiso);
        if (permiso == null)
        {
            throw new KeyNotFoundException($"No se encontró el permiso con ID {idPermiso}");
        }

        var resultado = await _permisoRepository.CambiarEstadoAsync(idPermiso, nuevoEstado);
        await _permisoRepository.SaveChangesAsync();

        return resultado;
    }

    public async Task<IEnumerable<string>> GetModulosAsync()
    {
        return await _permisoRepository.GetModulosAsync();
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    public async Task<bool> NombrePermisoExisteAsync(string nombrePermiso, int? idPermisoExcluir = null)
    {
        return await _permisoRepository.ExisteNombrePermisoAsync(nombrePermiso, idPermisoExcluir);
    }
}
