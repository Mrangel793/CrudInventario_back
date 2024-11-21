using InventarioApi.Models;
using InventarioApi.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {

            _context = context;
        }

        //Estado de conexion con el servidor
        [HttpGet]
        [Route("ConexionServidor")]
        public async Task<ActionResult<string>> GetConexionServidor()
        {
            return Ok("Conectado");
        }

        //Estados de conexion con la tabla de la base de datos
        [HttpGet]
        [Route("ConexionDB")]
        public async Task<ActionResult<string>> GetConexionDB()
        {
            try
            {
                var usuarios = await _context.productos.ToListAsync();
                return Ok("Buena Calidad");

            }
            catch (Exception ex)
            {
                return BadRequest("Mala calidad");
            }
        }
        //Aqui comienza el CRUD
        //Trae listado de productos
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            var lista = await _context.productos.ToListAsync(); 
            return Ok(lista);
        }

        //Buscar producto por id
        [HttpGet]
        [Route("ConsultarId/{id}")]
        public async Task<ActionResult<List<Producto>>> GetSingleProducto(int id)
        {
            var product = await _context.productos.FirstOrDefaultAsync(ob => ob.Id == id);

            if (product == null) {
                return NotFound("No se encontro el producto");
            }
            return Ok(product);
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<string>> CreateProduct(Producto product)
        {
            try
            {
                _context.productos.Add(product);
                await _context.SaveChangesAsync();
                return Ok("Producto creado con exito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error durante el proceso de almacenamiento");
            }

        }

        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult<List<Producto>>> UpdateProduct(Producto product)
        {
            var producto = await _context.productos.FindAsync(product.Id);
            if (producto == null) {
                return BadRequest("Producto no encontrado");
            }
            producto.Nombre = product.Nombre;
            producto.Descripcion = product.Descripcion;
            producto.Valor = product.Valor;
            await _context.SaveChangesAsync();
            return Ok(await _context.productos.ToListAsync());
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<ActionResult<string>> DeleteProduct(int id)
        {
            var Product = await _context.productos.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (Product == null) {
                return NotFound("El producto no existe");
            }
            try
            {
                _context.productos.Remove(Product);
                await _context.SaveChangesAsync();
                return Ok("Producto eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("No fue posible eliminar el producto");
            }
        }
    }
}
