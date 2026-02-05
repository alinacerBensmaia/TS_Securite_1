
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TS5N211_CbFonctionCommune.AccessBD
{
    public class EnregistrerReglesAAppliquer
    {
        private string mMessage;

        public EnregistrerReglesAAppliquer()
        {
        }

        public string Message
        {
            get
            {
                return mMessage;
            }

            set
            {
                mMessage = value;
            }
        }

        //public string Message { get => mMessage; set => mMessage = value; }

        public bool ExecuterRequeteInsert(DataTable donnees)
        {
            try
            {
                donnees.TableName = TsCaTablesChamps.REGNTFS;

                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(120)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                       
                        TsCaOutils.ExecuterTraitementBulk(connection, donnees);
                    }
                    tScope.Complete();
                }

            }
            catch (Exception ex)
            {
                Message= "ERREUR --> Erreur dans EnregistrerReglesAAppliquer.ExecuterRequete(): " + ex.Message + Environment.NewLine; 
                return  false;
            }
            return true;
        }

        public void ViderTable()
        {
            string sRequete = $"DELETE FROM {TsCaTablesChamps.REGNTFS}";

            try
            {
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        SqlCommand commandeSql = new SqlCommand(sRequete, connection);

                        commandeSql.ExecuteNonQuery();
                    }

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> ViderTable(): " +
                                          ex.Message +
                                          Environment.NewLine);

            }

        }
    }
}