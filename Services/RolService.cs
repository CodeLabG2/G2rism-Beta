using AutoMapper;
using G2rismBeta.API.DTOs.Rol;
using G2rismBeta.API.DTOs.RolPermiso;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio que contiene toda la lógica de negocio para gestión de Roles
/// </summary>
public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;
    private readonly IPermisoRepository _permisoRepository;
    private readonly IRolPermisoRepository _rolPermisoRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor: Recibe los repositories necesarios y AutoMapper
    /// </summary>
    public RolService(
        IRolRepository rolRepository,
        IPermisoRepository permisoRepository,
        IRolPermisoRepository rolPermisoRepository,
        IMapper mapper)
    {
        _rolRepository = rolRepository;
        _permisoRepository = permisoRepository;
        _rolPermisoRepository = rolPermisoRepository;
        _mapper = mapper;
    }

    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todos los roles del sistema
    /// </summary>
    public async Task<IEnumerable<RolResponseDto>> GetAllRolesAsync()
    {
        // 1. Obtener roles del repository
        var roles = await _rolRepository.GetAllAsync();

        // 2. Convertir de Model a DTO usando AutoMapper
        var rolesDto = _mapper.Map<IEnumerable<RolResponseDto>>(roles);

        return rolesDto;
    }

    /// <summary>
    /// Obtener solo roles activos (Estado = true)
    /// </summary>
    public async Task<IEnumerable<RolResponseDto>> GetRolesActivosAsync()
    {
        var roles = await _rolRepository.GetRolesActivosAsync();
        return _mapper.Map<IEnumerable<RolResponseDto>>(roles);
    }

    /// <summary>
    /// Obtener un rol específico por su ID
    /// </summary>
    public async Task<RolResponseDto?> GetRolByIdAsync(int idRol)
    {
        // 1. Validar que el ID sea positivo
        if (idRol <= 0)
        {
            throw new ArgumentException("El ID del rol debe ser mayor a 0", nameof(idRol));
        }

        // 2. Buscar el rol
        var rol = await _rolRepository.GetByIdAsync(idRol);

        // 3. Si no existe, retornar null
        if (rol == null)
        {
            return null;
        }

        // 4. Convertir a DTO y retornar
        return _mapper.Map<RolResponseDto>(rol);
    }

    /// <summary>
    /// Obtener un rol con todos sus permisos incluidos
    /// </summary>
    public async Task<RolConPermisosDto?> GetRolConPermisosAsync(int idRol)
    {
        // 1. Validación
        if (idRol <= 0)
        {
            throw new ArgumentException("El ID del rol debe ser mayor a 0", nameof(idRol));
        }

        // 2. Obtener rol con permisos del repository
        var rol = await _rolRepository.GetRolConPermisosAsync(idRol);

        if (rol == null)
        {
            return null;
        }

        // 3. Mapear a DTO especial que incluye los permisos
        return _mapper.Map<RolConPermisosDto>(rol);
    }

    /// <summary>
    /// Crear un nuevo rol en el sistema
    /// </summary>
    public async Task<RolResponseDto> CreateRolAsync(RolCreateDto rolCreateDto)
    {
        // ========================================
        // VALIDACIONES DE NEGOCIO
        // ========================================

        // 1. Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(rolCreateDto.Nombre))
        {
            throw new ArgumentException("El nombre del rol es obligatorio");
        }

        // 2. Validar que el nombre no exista
        var nombreExiste = await _rolRepository.ExisteNombreAsync(rolCreateDto.Nombre);
        if (nombreExiste)
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{rolCreateDto.Nombre}'");
        }

        // 3. Validar nivel de acceso
        if (rolCreateDto.NivelAcceso < 1 || rolCreateDto.NivelAcceso > 100)
        {
            throw new ArgumentException("El nivel de acceso debe estar entre 1 y 100");
        }

        // ========================================
        // CREAR EL ROL
        // ========================================

        // 4. Convertir DTO a Model
        var nuevoRol = _mapper.Map<Rol>(rolCreateDto);

        // 5. Establecer valores por defecto
        nuevoRol.Estado = true;
        nuevoRol.FechaCreacion = DateTime.Now;

        // 6. Guardar en la base de datos
        var rolCreado = await _rolRepository.AddAsync(nuevoRol);
        await _rolRepository.SaveChangesAsync();

        // 7. Convertir a DTO y retornar
        return _mapper.Map<RolResponseDto>(rolCreado);
    }

    /// <summary>
    /// Actualizar un rol existente (soporta actualizaciones parciales)
    /// Solo actualiza los campos que se envíen en el DTO (no nulos)
    /// </summary>
    public async Task<RolResponseDto> UpdateRolAsync(int idRol, RolUpdateDto rolUpdateDto)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        // 1. Verificar que el rol existe
        var rolExistente = await _rolRepository.GetByIdAsync(idRol);
        if (rolExistente == null)
        {
            throw new KeyNotFoundException($"No se encontró el rol con ID {idRol}");
        }

        // 2. Validar que al menos un campo se está actualizando
        if (rolUpdateDto.Nombre == null &&
            rolUpdateDto.Descripcion == null &&
            rolUpdateDto.NivelAcceso == null &&
            rolUpdateDto.Estado == null)
        {
            throw new ArgumentException("Debe proporcionar al menos un campo para actualizar");
        }

        // 3. Si se está actualizando el nombre, validar que no esté vacío
        if (rolUpdateDto.Nombre != null)
        {
            if (string.IsNullOrWhiteSpace(rolUpdateDto.Nombre))
            {
                throw new ArgumentException("El nombre del rol no puede estar vacío");
            }

            // 4. Validar que el nuevo nombre no exista (excluyendo el rol actual)
            var nombreExiste = await _rolRepository.ExisteNombreAsync(
                rolUpdateDto.Nombre,
                idRol);
            if (nombreExiste)
            {
                throw new InvalidOperationException($"Ya existe otro rol con el nombre '{rolUpdateDto.Nombre}'");
            }
        }

        // 5. Validar nivel de acceso si se está actualizando
        if (rolUpdateDto.NivelAcceso.HasValue)
        {
            if (rolUpdateDto.NivelAcceso < 1 || rolUpdateDto.NivelAcceso > 100)
            {
                throw new ArgumentException("El nivel de acceso debe estar entre 1 y 100");
            }
        }

        // ========================================
        // ACTUALIZAR EL ROL (SOLO CAMPOS NO NULOS)
        // ========================================

        // 6. Actualizar solo los campos que fueron proporcionados
        if (rolUpdateDto.Nombre != null)
        {
            rolExistente.Nombre = rolUpdateDto.Nombre;
        }

        if (rolUpdateDto.Descripcion != null)
        {
            rolExistente.Descripcion = rolUpdateDto.Descripcion;
        }

        if (rolUpdateDto.NivelAcceso.HasValue)
        {
            rolExistente.NivelAcceso = rolUpdateDto.NivelAcceso.Value;
        }

        if (rolUpdateDto.Estado.HasValue)
        {
            rolExistente.Estado = rolUpdateDto.Estado.Value;
        }

        rolExistente.FechaModificacion = DateTime.Now;

        // 7. Guardar cambios
        await _rolRepository.UpdateAsync(rolExistente);
        await _rolRepository.SaveChangesAsync();

        // 8. Retornar rol actualizado
        return _mapper.Map<RolResponseDto>(rolExistente);
    }

    /// <summary>
    /// Eliminar un rol (validando que no tenga usuarios)
    /// </summary>
    public async Task<bool> DeleteRolAsync(int idRol)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        // 1. Verificar que el rol existe
        var rol = await _rolRepository.GetByIdAsync(idRol);
        if (rol == null)
        {
            throw new KeyNotFoundException($"No se encontró el rol con ID {idRol}");
        }

        // 2. VALIDACIÓN IMPORTANTE: Verificar que no tenga usuarios asignados
        // TODO: Cuando implementen el módulo de Usuarios, agregar esta validación:
        // var tieneUsuarios = await _usuarioRepository.ExistenUsuariosConRolAsync(idRol);
        // if (tieneUsuarios)
        // {
        //     throw new InvalidOperationException("No se puede eliminar el rol porque tiene usuarios asignados");
        // }

        // 3. Validación: No permitir eliminar roles críticos del sistema
        var rolesProtegidos = new[] { "Administrador", "Super Administrador" };
        if (rolesProtegidos.Contains(rol.Nombre))
        {
            throw new InvalidOperationException($"El rol '{rol.Nombre}' es un rol del sistema y no se puede eliminar");
        }

        // ========================================
        // ELIMINAR
        // ========================================

        // 4. Primero eliminar las asignaciones de permisos
        await _rolPermisoRepository.RemoverTodosLosPermisosAsync(idRol);
        await _rolPermisoRepository.SaveChangesAsync();

        // 5. Luego eliminar el rol
        var resultado = await _rolRepository.DeleteAsync(idRol);
        await _rolRepository.SaveChangesAsync();

        return resultado;
    }

    // ========================================
    // OPERACIONES ESPECIALES
    // ========================================

    /// <summary>
    /// Activar o desactivar un rol
    /// </summary>
    public async Task<bool> CambiarEstadoRolAsync(int idRol, bool nuevoEstado)
    {
        // 1. Verificar que el rol existe
        var rol = await _rolRepository.GetByIdAsync(idRol);
        if (rol == null)
        {
            throw new KeyNotFoundException($"No se encontró el rol con ID {idRol}");
        }

        // 2. Validar roles protegidos
        if (!nuevoEstado) // Si se va a desactivar
        {
            var rolesProtegidos = new[] { "Administrador", "Super Administrador" };
            if (rolesProtegidos.Contains(rol.Nombre))
            {
                throw new InvalidOperationException($"El rol '{rol.Nombre}' no se puede desactivar");
            }
        }

        // 3. Cambiar estado
        var resultado = await _rolRepository.CambiarEstadoAsync(idRol, nuevoEstado);
        await _rolRepository.SaveChangesAsync();

        return resultado;
    }

    /// <summary>
    /// Asignar múltiples permisos a un rol
    /// IMPORTANTE: Solo agrega permisos nuevos, NO elimina los existentes
    /// Retorna la cantidad de permisos nuevos agregados
    /// </summary>
    public async Task<int> AsignarPermisosAsync(int idRol, List<int> idsPermisos)
    {
        // ========================================
        // VALIDACIONES
        // ========================================

        // 1. Verificar que el rol existe
        var rol = await _rolRepository.GetByIdAsync(idRol);
        if (rol == null)
        {
            throw new KeyNotFoundException($"No se encontró el rol con ID {idRol}");
        }

        // 2. Validar que la lista no esté vacía
        if (idsPermisos == null || !idsPermisos.Any())
        {
            throw new ArgumentException("Debe proporcionar al menos un permiso");
        }

        // 3. Validar que todos los permisos existen
        foreach (var idPermiso in idsPermisos)
        {
            var permisoExiste = await _permisoRepository.ExistsAsync(idPermiso);
            if (!permisoExiste)
            {
                throw new KeyNotFoundException($"No se encontró el permiso con ID {idPermiso}");
            }
        }

        // ========================================
        // ASIGNAR PERMISOS
        // ========================================

        // 4. Asignar todos los permisos (solo agrega los nuevos, mantiene los existentes)
        var cantidadAgregados = await _rolPermisoRepository.AsignarPermisosMultiplesAsync(idRol, idsPermisos);
        await _rolPermisoRepository.SaveChangesAsync();

        return cantidadAgregados;
    }

    /// <summary>
    /// Remover un permiso específico de un rol
    /// </summary>
    public async Task<bool> RemoverPermisoAsync(int idRol, int idPermiso)
    {
        // 1. Verificar que el rol existe
        var rol = await _rolRepository.GetByIdAsync(idRol);
        if (rol == null)
        {
            throw new KeyNotFoundException($"No se encontró el rol con ID {idRol}");
        }

        // 2. Verificar que el permiso existe
        var permiso = await _permisoRepository.GetByIdAsync(idPermiso);
        if (permiso == null)
        {
            throw new KeyNotFoundException($"No se encontró el permiso con ID {idPermiso}");
        }

        // 3. Verificar que el rol tiene ese permiso
        var tienePermiso = await _rolPermisoRepository.RolTienePermisoAsync(idRol, idPermiso);
        if (!tienePermiso)
        {
            throw new InvalidOperationException("El rol no tiene asignado ese permiso");
        }

        // 4. Remover el permiso
        var resultado = await _rolPermisoRepository.RemoverPermisoAsync(idRol, idPermiso);
        await _rolPermisoRepository.SaveChangesAsync();

        return resultado;
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Verificar si un nombre de rol ya existe
    /// </summary>
    public async Task<bool> NombreRolExisteAsync(string nombre, int? idRolExcluir = null)
    {
        return await _rolRepository.ExisteNombreAsync(nombre, idRolExcluir);
    }
}
