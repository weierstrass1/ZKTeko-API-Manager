using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ZKTekoLibrary.DAO
{
    public class Registro
    {
        public const string TABLE_NAME = "checkinout";
        public const string ID_COLUMN_NAME = "id";
        public const string PERSON_ID_COLUMN_NAME = "userid";
        public const string SERIAL_NUMBER_COLUMN_NAME = "SN";
        public const string DATE_COLUMN_NAME = "checktime";
        public const string TYPE_COLUMN_NAME = "checktype";
        public const string STATUS_COLUMN_NAME = "Respuesta";
        public const string HOUR_DIFF_COLUMN_NAME = "Respuesta";

        public long ID { get; private set; }
        public string IDPersona { get; private set; }
        public string NumeroSerial { get; private set; }
        public string Tipo { get; private set; }
        public string Estado { get; internal set; }
        public string Fecha { get; private set; }
        public string Hora { get; private set; }

        public static void UpdateStatus(Registro reg, string newStatus)
        {
            string cmd = $"update {TABLE_NAME} set {STATUS_COLUMN_NAME} = '{newStatus}' where {ID_COLUMN_NAME} = {reg.ID}";

            ZKTekoAPIManager.sqlmanager.DoCommand(cmd);
        }

        public static List<Registro> GetAllWithIgnore()
        {
            string where = getWhere() +
                $"and ({ STATUS_COLUMN_NAME} not in { Settings.DBIgnoreStatus} OR { STATUS_COLUMN_NAME} IS NULL) ";
            return GetAll(where);
        }
        public static List<Registro> GetAll()
        {
            string where = getWhere();

            return GetAll(where);
        }
        private static string getWhere()
        {          
            string where;

            if (Settings.QueryDateRange == null || Settings.QueryDateRange.Count < 2)
            {
                DateTime now = DateTime.Now;
                DateTime DaysAgo = now.AddDays(-Settings.QueryDays);

                string DaysAgoString = DaysAgo.ToString("yyyy-MM-dd 00:00:00");

                where = $"where DATEDIFF(DAY, '{DaysAgoString}',{DATE_COLUMN_NAME}) >= 0 ";
            }
            else
            {
                where = $"where {DATE_COLUMN_NAME} between '{Settings.QueryDateRange[0]}' and '{Settings.QueryDateRange[1]}' ";
            }

            if(Settings.UseSerialNumberList)
                where += $"and {SERIAL_NUMBER_COLUMN_NAME} in {Settings.SerialsNumberList} ";
            if (Settings.OnlyNewRegisters)
                where += $"and {STATUS_COLUMN_NAME} IS NULL ";
            else if(Settings.OnlyNotNull)
                where += $"and {STATUS_COLUMN_NAME} IS NOT NULL ";
            return where;
        }
        public static List<Registro> GetAll(string where)
        {
            string query = $"select {ID_COLUMN_NAME}, {PERSON_ID_COLUMN_NAME}, {SERIAL_NUMBER_COLUMN_NAME}, FORMAT ({DATE_COLUMN_NAME} ,'dd/MM/yyyy HH:mm:ss') as {DATE_COLUMN_NAME}, {TYPE_COLUMN_NAME}, {STATUS_COLUMN_NAME} " +
                        $"from {TABLE_NAME} "+
                        $"{where}"+
                        $"order by {DATE_COLUMN_NAME} asc";

            DataTable dt = ZKTekoAPIManager.sqlmanager.DoQuery(query);

            List<Registro> regs = new List<Registro>();
            Registro reg;

            string[] recieveFecha;

            foreach(DataRow row in dt.Rows)
            {

                recieveFecha = row[DATE_COLUMN_NAME].ToString().Trim().Split(' ');
                reg = new Registro
                {
                    ID = long.Parse(row[ID_COLUMN_NAME].ToString()),
                    IDPersona = row[PERSON_ID_COLUMN_NAME].ToString().Trim(),
                    NumeroSerial = row[SERIAL_NUMBER_COLUMN_NAME].ToString().Trim(),
                    Fecha = recieveFecha[0],
                    Hora = recieveFecha[1].Replace(".000", ""),
                    Estado = row[STATUS_COLUMN_NAME].ToString().Trim(),
                    Tipo = "entrada"
                };

                if (reg.Hora.Split(':')[0].Length < 2)
                    reg.Hora = "0" + reg.Hora;

                if (int.Parse(row[TYPE_COLUMN_NAME].ToString().Trim()) > 0)
                    reg.Tipo = "salida";

                regs.Add(reg);
            }
            return regs;
        }

        public override string ToString()
        {
            return $"{Fecha} {Hora} : (id={ID}, user={IDPersona}, serial={NumeroSerial}, type={Tipo}, status={Estado})";
        }
    }
}
