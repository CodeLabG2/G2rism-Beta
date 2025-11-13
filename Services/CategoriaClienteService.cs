using AutoMapper;
using G2rismBeta.API.DTOs.CategoriaCliente;
using G2rismBeta.API.Interfaces;
using G2rismBeta.API.Models;

namespace G2rismBeta.API.Services;

/// <summary>
/// Servicio que contiene toda la lógica de negocio para gestión de Categorías de Cliente (CRM)
/// </summary>
public class CategoriaClienteService : ICategoriaClienteService
{
    private readonly ICategoriaClienteRepository _categoriaRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor: Recibe el repository necesario y AutoMapper
    /// </summary>
    public CategoriaClienteService(
        ICategoriaClienteRepository categoriaRepository,
        IMapper mapper)
    {
        _categoriaRepository = categoriaRepository;
        _mapper = mapper;
    }

    // ========================================
    // OPERACIONES CRUD BÁSICAS
    // ========================================

    /// <summary>
    /// Obtener todas las categorías de cliente del sistema
    /// Incluye el conteo de clientes por categoría
    /// </summary>
    public async Task<IEnumerable<CategoriaClienteResponseDto>> GetAllCategoriasAsync()
    {
        // 1. Obtener categorías del repository
        var categorias = await _categoriaRepository.GetAllAsync();

        // 2. Convertir de Model a DTO usando AutoMapper
        var categoriasDto = _mapper.Map<IEnumerable<CategoriaClienteResponseDto>>(categorias);

        // 3. Agregar el conteo de clientes a cada categoría
        foreach (var categoriaDto in categoriasDto)
        {
            categoriaDto.CantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(categoriaDto.IdCategoria);
        }

        return categoriasDto;
    }

    /// <summary>
    /// Obtener solo categorías activas (Estado = true)
    /// Ordenadas por descuento de mayor a menor
    /// </summary>
    public async Task<IEnumerable<CategoriaClienteResponseDto>> GetCategoriasActivasAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasActivasAsync();
        var categoriasDto = _mapper.Map<IEnumerable<CategoriaClienteResponseDto>>(categorias);

        // Agregar el conteo de clientes
        foreach (var categoriaDto in categoriasDto)
        {
            categoriaDto.CantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(categoriaDto.IdCategoria);
        }

        return categoriasDto;
    }

    /// <summary>
    /// Obtener una categoría específica por su ID
    /// </summary>
    public async Task<CategoriaClienteResponseDto?> GetCategoriaByIdAsync(int idCategoria)
    {
        // 1. Validar que el ID sea positivo
        if (idCategoria <= 0)
        {
            throw new ArgumentException("El ID de la categoría debe ser mayor a 0", nameof(idCategoria));
        }

        // 2. Buscar la categoría
        var categoria = await _categoriaRepository.GetByIdAsync(idCategoria);

        // 3. Si no existe, retornar null
        if (categoria == null)
        {
            return null;
        }

        // 4. Convertir a DTO
        var categoriaDto = _mapper.Map<CategoriaClienteResponseDto>(categoria);

        // 5. Agregar conteo de clientes
        categoriaDto.CantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(idCategoria);

        return categoriaDto;
    }

    /// <summary>
    /// Crear una nueva categoría de cliente
    /// </summary>
    public async Task<CategoriaClienteResponseDto> CreateCategoriaAsync(CategoriaClienteCreateDto categoriaCreateDto)
    {
        // 1. Validación de negocio: Verificar que el nombre no exista
        var nombreExiste = await _categoriaRepository.ExisteNombreAsync(categoriaCreateDto.Nombre);
        if (nombreExiste)
        {
            throw new InvalidOperationException($"Ya existe una categoría con el nombre '{categoriaCreateDto.Nombre}'");
        }

        // 2. Validar que el descuento esté en el rango válido
        if (categoriaCreateDto.DescuentoPorcentaje < 0 || categoriaCreateDto.DescuentoPorcentaje > 100)
        {
            throw new ArgumentException("El descuento debe estar entre 0 y 100", nameof(categoriaCreateDto.DescuentoPorcentaje));
        }

        // 3. Convertir DTO a Model
        var categoria = _mapper.Map<CategoriaCliente>(categoriaCreateDto);

        // 4. Guardar en la base de datos
        await _categoriaRepository.AddAsync(categoria);
        await _categoriaRepository.SaveChangesAsync();

        // 5. Convertir el resultado a DTO y retornar
        var categoriaDto = _mapper.Map<CategoriaClienteResponseDto>(categoria);
        categoriaDto.CantidadClientes = 0; // Una nueva categoría no tiene clientes

        return categoriaDto;
    }

    /// <summary>
    /// Actualizar una categoría existente
    /// </summary>
    public async Task<CategoriaClienteResponseDto> UpdateCategoriaAsync(int idCategoria, CategoriaClienteUpdateDto categoriaUpdateDto)
    {
        // 1. Validar que el ID coincida
        if (idCategoria != categoriaUpdateDto.IdCategoria)
        {
            throw new ArgumentException("El ID de la categoría no coincide con el ID de la URL");
        }

        // 2. Verificar que la categoría existe
        var categoriaExistente = await _categoriaRepository.GetByIdAsync(idCategoria);
        if (categoriaExistente == null)
        {
            throw new InvalidOperationException($"No existe una categoría con el ID {idCategoria}");
        }

        // 3. Validar que el nombre no esté duplicado (excluyendo la categoría actual)
        var nombreExiste = await _categoriaRepository.ExisteNombreAsync(
            categoriaUpdateDto.Nombre, 
            idCategoria
        );
        if (nombreExiste)
        {
            throw new InvalidOperationException($"Ya existe otra categoría con el nombre '{categoriaUpdateDto.Nombre}'");
        }

        // 4. Validar el descuento
        if (categoriaUpdateDto.DescuentoPorcentaje < 0 || categoriaUpdateDto.DescuentoPorcentaje > 100)
        {
            throw new ArgumentException("El descuento debe estar entre 0 y 100", nameof(categoriaUpdateDto.DescuentoPorcentaje));
        }

        // 5. Actualizar los campos
        _mapper.Map(categoriaUpdateDto, categoriaExistente);

        // 6. Guardar cambios
        await _categoriaRepository.UpdateAsync(categoriaExistente);
        await _categoriaRepository.SaveChangesAsync();

        // 7. Retornar la categoría actualizada
        var categoriaDto = _mapper.Map<CategoriaClienteResponseDto>(categoriaExistente);
        categoriaDto.CantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(idCategoria);

        return categoriaDto;
    }

    /// <summary>
    /// Eliminar una categoría (solo si no tiene clientes asignados)
    /// </summary>
    public async Task<bool> DeleteCategoriaAsync(int idCategoria)
    {
        // 1. Validar ID
        if (idCategoria <= 0)
        {
            throw new ArgumentException("El ID de la categoría debe ser mayor a 0", nameof(idCategoria));
        }

        // 2. Verificar que la categoría existe
        var categoria = await _categoriaRepository.GetByIdAsync(idCategoria);
        if (categoria == null)
        {
            throw new InvalidOperationException($"No existe una categoría con el ID {idCategoria}");
        }

        // 3. Verificar que no tenga clientes asignados
        var cantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(idCategoria);
        if (cantidadClientes > 0)
        {
            throw new InvalidOperationException(
                $"No se puede eliminar la categoría porque tiene {cantidadClientes} cliente(s) asignado(s). " +
                "Reasigne los clientes a otra categoría antes de eliminarla."
            );
        }

        // 4. Eliminar la categoría
        await _categoriaRepository.DeleteAsync(idCategoria);
        await _categoriaRepository.SaveChangesAsync();

        return true;
    }

    // ========================================
    // OPERACIONES ESPECIALES (CRM)
    // ========================================

    /// <summary>
    /// Cambiar el estado de una categoría (activar/desactivar)
    /// </summary>
    public async Task<bool> CambiarEstadoCategoriaAsync(int idCategoria, bool nuevoEstado)
    {
        // 1. Validar ID
        if (idCategoria <= 0)
        {
            throw new ArgumentException("El ID de la categoría debe ser mayor a 0", nameof(idCategoria));
        }

        // 2. Cambiar estado
        var resultado = await _categoriaRepository.CambiarEstadoAsync(idCategoria, nuevoEstado);
        
        if (!resultado)
        {
            throw new InvalidOperationException($"No se pudo cambiar el estado de la categoría con ID {idCategoria}");
        }

        // 3. Guardar cambios
        await _categoriaRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Obtener categorías ordenadas por descuento (mayor a menor)
    /// Útil para mostrar las mejores categorías primero
    /// </summary>
    public async Task<IEnumerable<CategoriaClienteResponseDto>> GetCategoriasOrdenadaPorDescuentoAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasOrdenadaPorDescuentoAsync();
        var categoriasDto = _mapper.Map<IEnumerable<CategoriaClienteResponseDto>>(categorias);

        // Agregar conteo de clientes
        foreach (var categoriaDto in categoriasDto)
        {
            categoriaDto.CantidadClientes = await _categoriaRepository.ContarClientesPorCategoriaAsync(categoriaDto.IdCategoria);
        }

        return categoriasDto;
    }

    // ========================================
    // VALIDACIONES
    // ========================================

    /// <summary>
    /// Validar si un nombre de categoría ya existe
    /// </summary>
    public async Task<bool> NombreCategoriaExisteAsync(string nombre, int? idCategoriaExcluir = null)
    {
        return await _categoriaRepository.ExisteNombreAsync(nombre, idCategoriaExcluir);
    }
}
