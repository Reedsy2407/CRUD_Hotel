using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using waProyectoEF.Models;

namespace waProyectoEF.Controllers
{
    public class HabitacionController : Controller
    {
        //Obtener la cadena de conexion
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        //Arreglo de habitaciones (Listar las habitaciones)
        public List<Habitacion> aHabitaciones()
        {
            List<Habitacion> aHabitaciones = new List<Habitacion>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARHABITACIONES", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aHabitaciones.Add(new Habitacion()
                {
                    ide_hab = int.Parse(dr[0].ToString()),
                    num_hab = dr[1].ToString(),
                    nom_tha = dr[2].ToString(),
                    cap_hab = int.Parse(dr[3].ToString()),
                    pre_hab = double.Parse(dr[4].ToString()),
                    nom_est = dr[5].ToString()

                });
            }
            cn.Close();
            return aHabitaciones;
        }


        //Arreglo de habitaciones (Arreglo con columnas originales)
        public List<HabitacionO> aHabitacionesO()
        {
            List<HabitacionO> aHabitacionesO = new List<HabitacionO>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARHABITACIONES_O", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aHabitacionesO.Add(new HabitacionO()
                {
                    ide_hab = int.Parse(dr[0].ToString()),
                    num_hab = dr[1].ToString(),
                    ide_tha = int.Parse(dr[2].ToString()),
                    cap_hab = int.Parse(dr[3].ToString()),
                    pre_hab = double.Parse(dr[4].ToString()),
                    ide_est = int.Parse(dr[5].ToString())

                });
            }
            cn.Close();
            return aHabitacionesO;
        }

        //Arreglo para estados (combobox)
        public List<Estado> aEstados()
        {
            List<Estado> aEstados = new List<Estado>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARESTADO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aEstados.Add(new Estado()
                {
                    ide_est = int.Parse(dr[0].ToString()),
                    nom_est = dr[1].ToString()
                });
            }
            cn.Close();
            return aEstados;
        }

        //Arreglo para habitaciones x estado
        public List<Habitacion> aHabitacionesxEstado(int est)
        {
            List<Habitacion> aHabitacionesxEstado = new List<Habitacion>();
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_LISTAHABITACIONESxESTADO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_EST", est);
            //cmd.Parameters.Add("@VEN", SqlDbType.Int).Value = ven;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aHabitacionesxEstado.Add(new Habitacion()
                {
                    ide_hab = int.Parse(dr[0].ToString()),
                    num_hab = dr[1].ToString(),
                    nom_tha = dr[2].ToString(),
                    cap_hab = int.Parse(dr[3].ToString()),
                    pre_hab = double.Parse(dr[4].ToString()),
                    nom_est = dr[5].ToString()

                });
            }
            cn.Close();
            return aHabitacionesxEstado;
        }

        //Arreglo para tipo (combobox)
        public List<Tipo> aTipos()
        {
            List<Tipo> aTipos = new List<Tipo>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARTIPO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aTipos.Add(new Tipo()
                {
                    ide_tha = int.Parse(dr[0].ToString()),
                    nom_tha = dr[1].ToString()
                });
            }
            cn.Close();
            return aTipos;
        }

        //Arreglo para habitaciones x tipo
        public List<Habitacion> aHabitacionesxTipo(int tip)
        {
            List<Habitacion> aHabitacionesxTipo = new List<Habitacion>();
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_LISTAHABITACIONESxTIPO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_THA", tip);
            //cmd.Parameters.Add("@VEN", SqlDbType.Int).Value = ven;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aHabitacionesxTipo.Add(new Habitacion()
                {
                    ide_hab = int.Parse(dr[0].ToString()),
                    num_hab = dr[1].ToString(),
                    nom_tha = dr[2].ToString(),
                    cap_hab = int.Parse(dr[3].ToString()),
                    pre_hab = double.Parse(dr[4].ToString()),
                    nom_est = dr[5].ToString()

                });
            }
            cn.Close();
            return aHabitacionesxTipo;
        }


        private int obtenerSiguienteCodigo()
        {
            int siguienteCodigo = 1; // Código inicial si no hay habitaciones registrados

            // Corregir la declaración del bloque using
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT MAX(IDE_HAB) FROM Habitaciones", cn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    siguienteCodigo = Convert.ToInt32(result) + 1;
                }
            }

            return siguienteCodigo;
        }

        // Método para validar si ya existe un número de habitación registrado
        private bool existeNumeroHabitacion(string num_hab)
        {
            bool existe = false;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Habitaciones WHERE NUM_HAB = @NUM_HAB", cn);
                cmd.Parameters.AddWithValue("@NUM_HAB", num_hab);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }



        //Nueva Habitación
        [HttpGet]
        public ActionResult nuevaHabitacion()
        {
            int siguienteCodigo = obtenerSiguienteCodigo();
            ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est");
            ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha");
            ViewBag.SiguienteCodigo = siguienteCodigo;
            return View(new HabitacionO());
        }

        [HttpPost]
        public ActionResult nuevaHabitacion(HabitacionO objH)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est", objH.ide_est);
                ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha", objH.ide_tha);
                ViewBag.SiguienteCodigo = objH.ide_hab;
                return View(objH);
            }

            // Validar si el número de habitación ya existe
            if (existeNumeroHabitacion(objH.num_hab))
            {
                ModelState.AddModelError("num_hab", "El número de habitación ya está registrado.");
                ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est", objH.ide_est);
                ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha", objH.ide_tha);
                ViewBag.SiguienteCodigo = objH.ide_hab;
                return View(objH);
            }

            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_NUEVAHABITACION", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NUM_HAB", objH.num_hab);
            cmd.Parameters.AddWithValue("@IDE_THA", objH.ide_tha);
            cmd.Parameters.AddWithValue("@CAP_HAB", objH.cap_hab);
            cmd.Parameters.AddWithValue("@PRE_HAB", objH.pre_hab);
            cmd.Parameters.AddWithValue("@IDE_EST", objH.ide_est);
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("listadoHabitaciones");

        }
        //Actualiza Habitación
        [HttpGet]
        public ActionResult actualizaHabitacion(int id)
        {
            HabitacionO objH = aHabitacionesO().Where(h => h.ide_hab == id).FirstOrDefault();

            ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est", objH.ide_est);
            ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha", objH.ide_tha);
            return View(objH);
        }
        [HttpPost]
        public ActionResult actualizaHabitacion(HabitacionO objH)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est", objH.ide_est);
                ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha", objH.ide_tha);
                return View(objH);
            }
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_ACTUALIZARHABITACION", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_HAB", objH.ide_hab);
            cmd.Parameters.AddWithValue("@NUM_HAB", objH.num_hab);
            cmd.Parameters.AddWithValue("@IDE_THA", objH.ide_tha);
            cmd.Parameters.AddWithValue("@CAP_HAB", objH.cap_hab);
            cmd.Parameters.AddWithValue("@PRE_HAB", objH.pre_hab);
            cmd.Parameters.AddWithValue("@IDE_EST", objH.ide_est);
            cmd.ExecuteNonQuery();


            cn.Close();
            ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est");
            ViewBag.tipos = new SelectList(aTipos(), "ide_tha", "nom_tha");
            return RedirectToAction("listadoHabitaciones");

        }

        //Elimina Cliente
        [HttpGet]
        public ActionResult eliminaHabitacion(int id)
        {
            HabitacionO objH = aHabitacionesO().Where(h => h.ide_hab == id).FirstOrDefault();
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ELIMINAHABITACION", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDE_HAB", objH.ide_hab);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al eliminar la habitación.");
                return View(objH);
            }
            cn.Close();
            return RedirectToAction("listadoHabitaciones");
        }

        //Listado de Clientes
        public ActionResult listadoHabitaciones()
        {
            return View(aHabitaciones());
        }

        public ActionResult listadoHabitacionesxEstado(int est = 0)
        {
            ViewBag.est = est;
            ViewBag.estados = new SelectList(aEstados(), "ide_est", "nom_est");
            return View(aHabitacionesxEstado(est));
        }

        public ActionResult listadoHabitacionesxTipo(int tip= 0)
        {
            ViewBag.tip = tip;
            ViewBag.tipos= new SelectList(aTipos(), "ide_tha", "nom_tha");
            return View(aHabitacionesxTipo(tip));
        }

        // GET: Factura
        public ActionResult Index()
        {
            return View();
        }

    }
}