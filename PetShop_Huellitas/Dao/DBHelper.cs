using System.Data;
using Microsoft.Data.SqlClient;

// Archivo para poder copiar en otros proyectos (no tiene namespace)
// Es un ayudante de consultas SQL
    public class DBHelper
    {
        
        public SqlDataReader traerDataReader(
                    string cad_cn, string nombreSP, params object[] parametros)
        {
            SqlConnection cnx = new SqlConnection(cad_cn);
            cnx.Open();

            SqlCommand cmd = new SqlCommand(nombreSP, cnx);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parametros.Length > 0)
            {
                poblarParametros(cmd, parametros);
            }

            SqlDataReader dr =
                cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return dr;
        }

        public void ejecutarCRUD(
                    string cad_cn, string nombreSP, params object[] parametros)
        {
            using (SqlConnection cnx = new SqlConnection(cad_cn))
            {
                cnx.Open();
                SqlCommand cmd = new SqlCommand(nombreSP, cnx);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros.Length > 0)
                {
                    poblarParametros(cmd, parametros);
                }

                cmd.ExecuteNonQuery();
                cnx.Close();
            }
        }

           public void poblarParametros(SqlCommand cmd, params object[] parametros)
        {
            int indice = 0;
            // descubra los parametros del procedimiento almacenado que
            // será ejecutado el sqlcommand
            SqlCommandBuilder.DeriveParameters(cmd);
            //
            foreach (SqlParameter item in cmd.Parameters)
            {
                if (item.ParameterName != "@RETURN_VALUE")
                {
                    item.Value = parametros[indice];
                    indice++;
                }
            }
        }


    }

