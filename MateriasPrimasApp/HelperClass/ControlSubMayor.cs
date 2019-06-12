using MateriasPrimasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MateriasPrimasApp.Data;

namespace MateriasPrimasApp.HelperClass
{
    public class ControlSubMayor
    {
        private readonly ApplicationDbContext _context;

        public ControlSubMayor(ApplicationDbContext context)
        {
            _context = context;
        }
        public void DarEntrada(int entradaId)
        {
            Entrada entrada = _context.Entrada.First(e => e.Id == entradaId);
            DarEntrada(entrada); 
        }

        public void DarEntrada(Entrada entrada)
        {
            List<DetalleDeEntrada> detalles = _context.DetallesDeEntradas.Where(d => d.EntradaId == entrada.Id).Include(p => p.Producto).ToList();

            foreach (var item in detalles)
            {
                Submayor sub = _context.Submayor.FirstOrDefault(s => s.AlmacenId == entrada.UnidadOrganizativaId && s.ProductoId == item.ProductoId);
                if (sub == null)
                {
                    sub = new Submayor
                    {
                        AlmacenId = entrada.UnidadOrganizativaId,
                        Cantidad = item.Cantidad,
                        ProductoId = item.ProductoId,
                        UnidadId = item.Producto.UnidadId

                    };
                    _context.Add(sub);
                }
                else
                {
                    sub.Cantidad += item.Cantidad;
                    _context.Update(sub);
                }
                _context.SaveChanges();
            }

        }

        public void ProcesarVenta(Venta venta)
        {
            var detalles = _context.DetallesDeVenta.Where(d => d.VentaId == venta.Id).Include(p => p.Producto).ToList();

            foreach (var item in detalles)
            {
                Submayor sub = _context.Submayor.FirstOrDefault(s => s.AlmacenId == venta.UnidadOrganizativaId && s.ProductoId == item.ProductoId);
                if (sub == null)
                {
                    //generar error, no se puede vender algo que no está en almacén
                }
                else if(sub.Cantidad >= item.Cantidad)
                {
                    sub.Cantidad -= item.Cantidad;
                    _context.Update(sub);
                    _context.SaveChanges();
                }
            }

        }

        public bool ProcesarTraslado(Transferencia transferencia)
        {
            bool Success = true;
            List<DetalleDeTransferencia> detalles = _context.DetallesDeTransferencia.Where(d => d.TransferenciaId == transferencia.Id).Include(p => p.Producto).ToList();

            foreach (var item in detalles)
            {
                Submayor sub = _context.Submayor.FirstOrDefault(s => s.AlmacenId == transferencia.OrigenId && s.ProductoId == item.ProductoId);
                if (sub != null)
                {
                    if (sub.Cantidad >= item.Cantidad)
                    {
                        sub.Cantidad -= item.Cantidad;
                        _context.Update(sub);
                        Submayor sub2 = _context.Submayor.FirstOrDefault(s => s.AlmacenId == transferencia.DestinoId && s.ProductoId == item.ProductoId);

                        if (sub2 == null)
                        {
                            sub2 = new Submayor
                            {
                                AlmacenId = transferencia.DestinoId,
                                Cantidad = item.Cantidad,
                                ProductoId = item.ProductoId,
                                UnidadId = item.Producto.UnidadId
                            };
                            _context.Add(sub2);
                        }
                        else
                        {
                            sub2.Cantidad += item.Cantidad;
                            _context.Update(sub2);
                        }
                    }
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                Success = false;
            }

            return Success;
        }

        public bool ProcesarProcesamiento(Procesamiento procesamiento)
        {
            var submayorMP = _context.Submayor.FirstOrDefault(s => s.ProductoId == procesamiento.ProductoId && s.AlmacenId == procesamiento.UnidadOrganizativaId);
            bool Success = true;

            foreach (var item in procesamiento.DetallesDeProcesamiento)
            {
                Submayor sub = _context.Submayor.FirstOrDefault(s => s.AlmacenId == procesamiento.UnidadOrganizativaId && s.ProductoId == item.DerivadoId);
                if (sub != null)
                {
                    sub.Cantidad += item.Cantidad;
                    _context.Update(sub);
                }
                else
                {
                    var um = _context.Producto.Find(item.DerivadoId);
                    var newsub = new Submayor()
                    {
                        AlmacenId = procesamiento.UnidadOrganizativaId,
                        Cantidad = item.Cantidad,
                        ProductoId = item.DerivadoId,
                        UnidadId = um.UnidadId
                    };
                    _context.Add(newsub);
                }
            }
            try
            {
                submayorMP.Cantidad -= procesamiento.Cantidad;
                _context.Update(submayorMP);
                _context.SaveChanges();
            }
            catch
            {
                Success = false;
            }

            return Success;
        }

        public void DarEntradaDetalle(DetalleDeEntrada detalle)
        {
            Entrada entrada = _context.Entrada.First(e => e.Id == detalle.EntradaId);
            Submayor sub = _context.Submayor.FirstOrDefault(s => s.AlmacenId == entrada.UnidadOrganizativaId && s.ProductoId == detalle.ProductoId);
            if (sub == null)
            {
                sub = new Submayor
                {
                    AlmacenId = entrada.UnidadOrganizativaId,
                    Cantidad = detalle.Cantidad,
                    ProductoId = detalle.ProductoId
                };
                _context.Add(sub);
            }
            else
            {
                sub.Cantidad += detalle.Cantidad;
                _context.Update(sub);
            }
            _context.SaveChanges();
        }
    }
}
