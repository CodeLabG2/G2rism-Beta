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
        nuevoPermiso.NombrePermiso = nombrePermiso;
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

        // ========================================
        // ACTUALIZAR (soporta actualizaciones parciales)
        // ========================================

        // Actualizar solo los campos proporcionados
        if (!string.IsNullOrWhiteSpace(permisoUpdateDto.Modulo))
        {
            permisoExistente.Modulo = permisoUpdateDto.Modulo;
        }

        if (!string.IsNullOrWhiteSpace(permisoUpdateDto.Accion))
        {
            permisoExistente.Accion = permisoUpdateDto.Accion;
        }

        // Si se actualizó módulo o acción, regenerar NombrePermiso
        if (!string.IsNullOrWhiteSpace(permisoUpdateDto.Modulo) || !string.IsNullOrWhiteSpace(permisoUpdateDto.Accion))
        {
            var nuevoNombrePermiso = $"{permisoExistente.Modulo.ToLower()}.{permisoExistente.Accion.ToLower()}";

            // Validar que el nuevo nombre no exista (excluyendo el actual)
            var nombreExiste = await _permisoRepository.ExisteNombrePermisoAsync(nuevoNombrePermiso, idPermiso);
            if (nombreExiste)
            {
                throw new InvalidOperationException($"Ya existe otro permiso con el nombre '{nuevoNombrePermiso}'");
            }

            permisoExistente.NombrePermiso = nuevoNombrePermiso;
        }

        if (permisoUpdateDto.Descripcion != null)
        {
            permisoExistente.Descripcion = permisoUpdateDto.Descripcion;
        }

        if (permisoUpdateDto.Estado.HasValue)
        {
            permisoExistente.Estado = permisoUpdateDto.Estado.Value;
        }

        permisoExistente.FechaModificacion = DateTime.Now;

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
    // BÚSQUEDAS ESPECÍFICAS
    // ========================================

    public async Task<PermisoResponseDto?> GetPermisoByModuloYAccionAsync(string modulo, string accion)
    {
        if (string.IsNullOrWhiteSpace(modulo))
        {
            throw new ArgumentException("El módulo es obligatorio", nameof(modulo));
        }

        if (string.IsNullOrWhiteSpace(accion))
        {
            throw new ArgumentException("La acción es obligatoria", nameof(accion));
        }

        var permiso = await _permisoRepository.GetByModuloYAccionAsync(modulo, accion);

        if (permiso == null)
        {
            return null;
        }

        return _mapper.Map<PermisoResponseDto>(permiso);
    }

    public async Task<IEnumerable<PermisoResponseDto>> BuscarPermisosAsync(string termino)
    {
        if (string.IsNullOrWhiteSpace(termino))
        {
            throw new ArgumentException("El término de búsqueda es obligatorio", nameof(termino));
        }

        var todosLosPermisos = await _permisoRepository.GetAllAsync();

        // Buscar en módulo, acción, nombre y descripción
        var permisosFiltrados = todosLosPermisos.Where(p =>
            p.Modulo.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
            p.Accion.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
            p.NombrePermiso.Contains(termino, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrEmpty(p.Descripcion) && p.Descripcion.Contains(termino, StringComparison.OrdinalIgnoreCase))
        );

        return _mapper.Map<IEnumerable<PermisoResponseDto>>(permisosFiltrados);
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
