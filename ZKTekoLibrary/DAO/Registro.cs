using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

            if (ZKTekoAPIManager.sqlmanager.DoCommand(cmd) <= 0)
                throw new ArgumentException($"{nameof(reg)} doesn't exist in SQL Database");
        }

        public static List<Registro> GetAll()
        {
            DateTime now = DateTime.Now;
            DateTime FiveDaysAgo = now.AddDays(-Settings.QueryDays);

            string FiveDaysAgoString = FiveDaysAgo.ToString("yyyy-MM-dd 00:00:00");

            string query = $"select {ID_COLUMN_NAME}, {PERSON_ID_COLUMN_NAME}, {SERIAL_NUMBER_COLUMN_NAME}, {DATE_COLUMN_NAME}, {TYPE_COLUMN_NAME}, {STATUS_COLUMN_NAME} from {TABLE_NAME} where DATEDIFF(DAY, '{FiveDaysAgoString}',{DATE_COLUMN_NAME}) >= 0 and {STATUS_COLUMN_NAME} not in {Settings.DBIgnoreStatus} order by {DATE_COLUMN_NAME} asc";

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
                    Fecha = string.Join("/", recieveFecha[0].Split('-').Reverse()),
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
