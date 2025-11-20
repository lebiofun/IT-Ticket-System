using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
//TODO
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Ticket_System.DataL
{
    public static class DB_Helper
    {
        private static readonly string databaseName =
    ConfigurationManager.AppSettings["DatabaseName"];
        private static readonly string masterstring = ConfigurationManager.AppSettings["Connectionstringmaster"];



        public static bool Restore_db()
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Файлы .bak (*.bak)|*.bak";
                    ofd.Title = "Выберите .bak";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string backupFile = ofd.FileName;
                        string databaseName = ConfigurationManager.AppSettings["DatabaseName"];
                        // TODO
                        if (Program.DbConnection.State != ConnectionState.Closed)
                            Program.DbConnection.Close();

                        using (SqlConnection con = new SqlConnection(masterstring))
                        {
                            con.Open();

                            string sql = $@"
                    -- kill all connections
                    DECLARE @kill VARCHAR(8000) = '';
                    SELECT @kill = @kill + 'KILL ' + CONVERT(VARCHAR(5), session_id) + ';'
                    FROM sys.dm_exec_sessions
                    WHERE database_id = DB_ID('{databaseName}');
                    EXEC(@kill);

                    -- set to single user
                    ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

                    -- restore
                    RESTORE DATABASE [{databaseName}]
                    FROM DISK = '{backupFile}'
                    WITH REPLACE;

                    -- back to multiuser
                    ALTER DATABASE [{databaseName}] SET MULTI_USER;
                ";

                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {

                                cmd.ExecuteNonQuery();
                            }

                            con.Close();
                        }

                        MessageBox.Show("База данных успешно восстановлена!",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Восстановление БД не успешно:\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool Backup_db()
        {
            try
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Выбери папку куда сохранить .bak";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return false;

                    string folderPath = fbd.SelectedPath;
                    string backupFile = Path.Combine(folderPath, $"{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                    string backupQuery = $@"
                    BACKUP DATABASE [{databaseName}]
                    TO DISK = '{backupFile}'
                    WITH FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                    try
                    {
                        using (var command = new SqlCommand(backupQuery, Program.DbConnection))
                        {
                            Program.DbConnection.Open();
                            command.ExecuteNonQuery();

                            MessageBox.Show($"Резервное копирование успешно!\nСохранено в: {backupFile}",
                                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Резервное копирование не удалось:\n{ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        if (Program.DbConnection.State == System.Data.ConnectionState.Open)
                            Program.DbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка резервного копирования: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (Program.DbConnection.State == System.Data.ConnectionState.Open)
                    Program.DbConnection.Close();
            }


        }
    }

}
