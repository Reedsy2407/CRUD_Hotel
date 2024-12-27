using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using waProyectoEF.Models;

namespace waProyectoEF.Controllers
{
    public class ClienteController : Controller
    {
        // Obtener la cadena de conexión
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // Arreglo de clientes (Listar los clientes)
        public List<Cliente> aClientes()
        {
            List<Cliente> aClientes = new List<Cliente>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARCLIENTES", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aClientes.Add(new Cliente()
                {
                    ide_cli = int.Parse(dr[0].ToString()),
                    nom_cli = dr[1].ToString(),
                    email = dr[2].ToString(),
                    nom_met = dr[3].ToString(),
                    dni_cli = dr[4].ToString()
                });
            }
            cn.Close();
            return aClientes;
        }

        // Arreglo de clientes (Arreglo con columnas originales)
        public List<ClienteO> aClientesO()
        {
            List<ClienteO> aClientesO = new List<ClienteO>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARCLIENTES_O", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aClientesO.Add(new ClienteO()
                {
                    ide_cli = int.Parse(dr[0].ToString()),
                    nom_cli = dr[1].ToString(),
                    ape_cli = dr[2].ToString(),
                    email = dr[3].ToString(),
                    ide_met = int.Parse(dr[4].ToString()),
                    dni_cli = dr[5].ToString()
                });
            }
            cn.Close();
            return aClientesO;
        }

        // Arreglo para métodos (combobox)
        public List<Metodo> aMetodos()
        {
            List<Metodo> aMetodos = new List<Metodo>();
            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_LISTARMETODO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aMetodos.Add(new Metodo()
                {
                    ide_met = int.Parse(dr[0].ToString()),
                    nom_met = dr[1].ToString()
                });
            }
            cn.Close();
            return aMetodos;
        }

        // Arreglo para clientes por método
        public List<Cliente> aClientesxMetodo(int met)
        {
            List<Cliente> aClientesxMetodo = new List<Cliente>();
            cn.Open();

            SqlCommand cmd = new SqlCommand("SP_LISTACLIENTESxMETODO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_MET", met);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                aClientesxMetodo.Add(new Cliente()
                {
                    ide_cli = int.Parse(dr[0].ToString()),
                    nom_cli = dr[1].ToString(),
                    email = dr[2].ToString(),
                    nom_met = dr[3].ToString(),
                    dni_cli = dr[4].ToString()
                });
            }
            cn.Close();
            return aClientesxMetodo;
        }

        private int obtenerSiguienteCodigo()
        {
            int siguienteCodigo = 1; // Código inicial si no hay clientes registrados

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT MAX(IDE_CLI) FROM Clientes", cn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    siguienteCodigo = Convert.ToInt32(result) + 1;
                }
            }

            return siguienteCodigo;
        }

        private bool existeDniCliente(string dni)
        {
            bool existe = false;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Clientes WHERE DNI_CLI = @DNI_CLI", cn);
                cmd.Parameters.AddWithValue("@DNI_CLI", dni);

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Nuevo Cliente
        [HttpGet]
        public ActionResult nuevoCliente()
        {
            int siguienteCodigo = obtenerSiguienteCodigo();
            ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met");
            ViewBag.SiguienteCodigo = siguienteCodigo;
            return View(new ClienteO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult nuevoCliente(ClienteO objC)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met", objC.ide_met);
                ViewBag.SiguienteCodigo = objC.ide_cli;
                return View(objC);
            }

            // Validar si el DNI del cliente ya existe
            if (existeDniCliente(objC.dni_cli))
            {
                ModelState.AddModelError("dni_cli", "El DNI del cliente ya está registrado.");
                ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met", objC.ide_met);
                ViewBag.SiguienteCodigo = objC.ide_cli;
                return View(objC);
            }

            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_NUEVOCLIENTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NOM_CLI", objC.nom_cli);
            cmd.Parameters.AddWithValue("@APE_CLI", objC.ape_cli);
            cmd.Parameters.AddWithValue("@EMAIL", objC.email);
            cmd.Parameters.AddWithValue("@IDE_MET", objC.ide_met);
            cmd.Parameters.AddWithValue("@DNI_CLI", objC.dni_cli);
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("listadoClientes");
        }

        // Actualiza Cliente
        [HttpGet]
        public ActionResult actualizaCliente(int id)
        {
            ClienteO objC = aClientesO().Where(c => c.ide_cli == id).FirstOrDefault();
            ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met", objC.ide_met);
            return View(objC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult actualizaCliente(ClienteO objC)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met", objC.ide_met);
                return View(objC);
            }

            cn.Open();
            SqlCommand cmd = new SqlCommand("SP_ACTUALIZARCLIENTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDE_CLI", objC.ide_cli);
            cmd.Parameters.AddWithValue("@NOM_CLI", objC.nom_cli);
            cmd.Parameters.AddWithValue("@APE_CLI", objC.ape_cli);
            cmd.Parameters.AddWithValue("@EMAIL", objC.email);
            cmd.Parameters.AddWithValue("@IDE_MET", objC.ide_met);
            cmd.Parameters.AddWithValue("@DNI_CLI", objC.dni_cli);
            cmd.ExecuteNonQuery();
            cn.Close();

            return RedirectToAction("listadoClientes");
        }

        // Elimina Cliente
        [HttpGet]
        public ActionResult eliminaCliente(int id)
        {
            ClienteO objC = aClientesO().Where(c => c.ide_cli == id).FirstOrDefault();
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ELIMINACLIENTE", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDE_CLI", objC.ide_cli);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al eliminar el cliente.");
                return View(objC);
            }
            cn.Close();
            return RedirectToAction("listadoClientes");
        }

        // Listado de Clientes
        public ActionResult listadoClientes()
        {
            return View(aClientes());
        }

        public ActionResult listadoClientesxMetodo(int met = 0)
        {
            ViewBag.met = met;
            ViewBag.metodos = new SelectList(aMetodos(), "ide_met", "nom_met");
            return View(aClientesxMetodo(met));
        }

        // GET: Factura
        public ActionResult Index()
        {
            return View();
        }
    }
}
