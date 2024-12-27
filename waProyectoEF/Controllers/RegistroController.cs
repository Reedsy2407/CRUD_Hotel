using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using waProyectoEF.Models;

namespace waProyectoEF.Controllers
{
    public class RegistroController : Controller
    {
        //Obtener la cadena de conexion
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        //Arreglo de registros de alquiler (Listar los registros de alquiler)
        public List<Registro> aRegistros()
        {
            List<Registro> aRegistros = new List<Registro>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTAREGISTROS", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aRegistros.Add(new Registro()
                {
                    ide_alq = int.Parse(dr[0].ToString()),
                    nom_cli = dr[1].ToString(),
                    dni_cli = dr[2].ToString(),
                    num_hab = dr[3].ToString(),
                    pre_hab = double.Parse(dr[4].ToString()),
                    fen_alq = DateTime.Parse(dr[5].ToString()),
                    fsa_alq = DateTime.Parse(dr[6].ToString())
                });
            }
            cn.Close();
            return aRegistros;
        }

        //Arreglo de registros de alquiler con columnas originales
        public List<RegistroO> aRegistrosO()
        {
            List<RegistroO> aRegistrosO = new List<RegistroO>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTAREGISTROS_O", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aRegistrosO.Add(new RegistroO()
                {
                    ide_alq = int.Parse(dr[0].ToString()),
                    ide_cli = int.Parse(dr[1].ToString()),
                    ide_hab = int.Parse(dr[2].ToString()),
                    fen_alq = DateTime.Parse(dr[3].ToString()),
                    fsa_alq = DateTime.Parse(dr[4].ToString()),
                });
            }
            cn.Close();
            return aRegistrosO;
        }

        //Arreglo para clientes (combobox)
        public List<Cliente> aClientes()
        {
            List<Cliente> aClientes = new List<Cliente>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARCLIENTES_R", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aClientes.Add(new Cliente()
                {
                    ide_cli = int.Parse(dr[0].ToString()),
                    nom_cli = dr[1].ToString()
                });
            }
            cn.Close();
            return aClientes;
        }

        //Arreglo para habitaciones (combobox)
        public List<Habitacion> aHabitaciones()
        {
            List<Habitacion> aHabitaciones = new List<Habitacion>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARHABITACIONES_R", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aHabitaciones.Add(new Habitacion()
                {
                    ide_hab = int.Parse(dr[0].ToString()),
                    num_hab = dr[1].ToString()
                });
            }
            cn.Close();
            return aHabitaciones;
        }

        private int obtenerSiguienteCodigo()
        {
            int siguienteCodigo = 1; // Código inicial si no hay alquileres registrados

            // Corregir la declaración del bloque using
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT MAX(IDE_ALQ) FROM Alquileres", cn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    siguienteCodigo = Convert.ToInt32(result) + 1;
                }
            }

            return siguienteCodigo;
        }


        //Nuevo Registro
        [HttpGet]
        public ActionResult nuevoRegistro()
        {
            int siguienteCodigo = obtenerSiguienteCodigo();
            ViewBag.clientes = new SelectList(aClientes(), "ide_cli", "nom_cli");
            ViewBag.habitaciones = new SelectList(aHabitaciones(), "ide_hab", "num_hab");
            ViewBag.SiguienteCodigo = siguienteCodigo;
            return View(new RegistroO());
        }

        [HttpPost]
        public ActionResult nuevoRegistro(RegistroO objR)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.clientes = new SelectList(aClientes(), "ide_cli", "nom_cli", objR.ide_cli);
                ViewBag.habitaciones = new SelectList(aHabitaciones(), "ide_hab", "num_hab", objR.ide_hab);
                ViewBag.SiguienteCodigo = objR.ide_alq;
                return View(objR);
            }
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_NUEVOREGISTRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_CLI", objR.ide_cli);
            cmd.Parameters.AddWithValue("@IDE_HAB", objR.ide_hab);
            cmd.Parameters.AddWithValue("@FEN_ALQ", objR.fen_alq);
            cmd.Parameters.AddWithValue("@FSA_ALQ", objR.fsa_alq);
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("listadoRegistros");

        }
        //Actualiza Registro
        [HttpGet]
        public ActionResult actualizaRegistro(int id)
        {
            RegistroO objR = aRegistrosO().Where(r => r.ide_alq == id).FirstOrDefault();

            ViewBag.clientes = new SelectList(aClientes(), "ide_cli", "nom_cli", objR.ide_cli);
            ViewBag.habitaciones = new SelectList(aHabitaciones(), "ide_hab", "num_hab", objR.ide_hab);
            return View(objR);
        }
        [HttpPost]
        public ActionResult actualizaRegistro(RegistroO objR)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.clientes = new SelectList(aClientes(), "ide_cli", "nom_cli", objR.ide_cli);
                ViewBag.habitaciones = new SelectList(aHabitaciones(), "ide_hab", "num_hab", objR.ide_hab);
                return View(objR);
            }
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_ACTUALIZAREGISTRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_ALQ", objR.ide_alq);
            cmd.Parameters.AddWithValue("@IDE_CLI", objR.ide_cli);
            cmd.Parameters.AddWithValue("@IDE_HAB", objR.ide_hab);
            cmd.Parameters.AddWithValue("@FEN_ALQ", objR.fen_alq);
            cmd.Parameters.AddWithValue("@FSA_ALQ", objR.fsa_alq);
            cmd.ExecuteNonQuery();


            cn.Close();
            ViewBag.clientes = new SelectList(aClientes(), "ide_cli", "nom_cli");
            ViewBag.habitaciones = new SelectList(aHabitaciones(), "ide_hab", "num_hab");
            return RedirectToAction("listadoRegistros");

        }
        //Elimina Registro
        [HttpGet]
        public ActionResult eliminaRegistro(int id)
        {
            RegistroO objR = aRegistrosO().Where(r => r.ide_alq == id).FirstOrDefault();
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ELIMINARALQUILER", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDE_ALQ", objR.ide_alq);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al eliminar el cliente.");
                return View(objR);
            }
            cn.Close();
            return RedirectToAction("listadoRegistros");
        }

        //Listado de registros
        public ActionResult listadoRegistros()
        {
            return View(aRegistros());
        }

        // GET: Factura
        public ActionResult Index()
        {
            return View();
        }
    }
}