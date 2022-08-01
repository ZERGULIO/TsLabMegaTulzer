using Microsoft.Data.Sqlite;
using System.Text.Json;
using System.Xml;

namespace TsLab_Agent_Control
{
    public partial class Form1 : Form
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TSLab\\TSLab 2.0\\TSLab.sqlite";
        public Form1()
        {
            InitializeComponent();
            using (var connection = new SqliteConnection("Data Source=" + path))
            {
                try
                {
                    connection.Open();
                    label_db_status.Text = "Database Connection = ОК - " + path;
                    label_db_status.ForeColor = Color.Green;
                }
                catch (Exception)
                {
                    label_db_status.Text = "Database Connection = ERROR!!! - " + path;
                    label_db_status.ForeColor = Color.Red;
                    MessageBox.Show("Не найден файл БД - TSLab.sqlite");
                }
            }
            }
        public void button1_Click(object sender, EventArgs e) // Интервал
        {
            Set_DATA("Interval", textBox2.Text);
        }
        public void Set_DATA_Json(string block_name, string value)
        {
            using (var connection = new SqliteConnection("Data Source=" + path))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM Script";

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        int i = 0;
                        while (reader.Read())   // построчно считываем данные
                        {
                            var row_id = reader.GetValue(0);
                            var json_data = reader.GetValue(6);
                            if (json_data == DBNull.Value)
                            {
                                continue;
                            }
                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json_data.ToString());

                            string tx = jsonObj["Options"][block_name] = value;

                            string out_json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                            //MessageBox.Show(out_json);
                            //break;
                            string sqlExpression2 = "UPDATE Script SET Code = '" + out_json + "' WHERE ScriptId = " + row_id;
                            command = new SqliteCommand(sqlExpression2, connection);
                            int number = command.ExecuteNonQuery();

                            i++;
                            string message = i + " - Script Name: " + reader.GetValue(2);
                            
                        }
                    }
                }
            }
        }
        public void Set_DATA(string block_name, string value)
        {
            using (var connection = new SqliteConnection("Data Source=" + path))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM Script";

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        int i = 0;
                        while (reader.Read())   // построчно считываем данные
                        {
                            var row_id = reader.GetValue(0);
                            var xml_data = reader.GetValue(3);
                            if (xml_data == DBNull.Value)
                            {
                                //Console.WriteLine("No Data in Table");
                                continue;
                            }
                            var doc = new XmlDocument();
                            doc.LoadXml((string)xml_data);

                            var xml_elem = doc.DocumentElement.GetElementsByTagName(block_name);

                            if (xml_elem.Count != 0)
                            {
                                xml_elem[0].InnerText = value;
                                //MessageBox.Show(xml_elem[0].InnerText);
                                string sqlExpression2 = "UPDATE Script SET Code = '" + doc.OuterXml + "' WHERE ScriptId = " + row_id;
                                command = new SqliteCommand(sqlExpression2, connection);
                                int number = command.ExecuteNonQuery();

                                i++;
                                string message = i + " - Script Name: " + reader.GetValue(2);
                            }
                        }
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e) // Дата от
        {
            Set_DATA("DateFrom", textBox1.Text + "T00:00:00");
        }
        private void button3_Click(object sender, EventArgs e) // Исп. дату от
        {
            Set_DATA("UseDateFrom", checkBox1.Checked == true ? "true" : "false" );
        }
        private void button4_Click(object sender, EventArgs e) // Исп. дату к
        {
            Set_DATA("UseDateTo", checkBox2.Checked == true ? "true" : "false");
        }
        private void button5_Click(object sender, EventArgs e) // Дата к
        {
            Set_DATA("DateTo", textBox3.Text + "T00:00:00");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Set_DATA("DateReload", textBox4.Text + "T00:00:00");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Set_DATA("UseDateReload", checkBox3.Checked == true ? "true" : "false");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Set_DATA("SessionBegin", "2000-01-01T" + textBox5.Text + ":00");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Set_DATA("SessionEnd", "2000-01-01T" + textBox6.Text + ":00");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Set_DATA("MaxCandels", textBox7.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("DefEntryApprove", checkBox4.Checked == true ? "true" : "false");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("DefExitApprove", checkBox5.Checked == true ? "true" : "false");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("AutoEntryBars", textBox8.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("AutoEntryIgnoreByMarketAsLimit", checkBox6.Checked == true ? "true" : "false");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("AutoCloseBars", textBox9.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("AutoCloseIgnoreByMarketAsLimit", checkBox7.Checked == true ? "true" : "false");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("RemoveInactivePositions", checkBox8.Checked == true ? "true" : "false");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("WarnSkippedOpenPositions", checkBox9.Checked == true ? "true" : "false");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("NotOpenIfHasSkippedExit", checkBox10.Checked == true ? "true" : "false");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("NoCalcInfo", checkBox11.Checked == true ? "true" : "false");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("MaxBarsForSignal", textBox10.Text);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("ExitSignalOnlyForLastBar", checkBox12.Checked == true ? "true" : "false");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("WaitExecutionExitBars", textBox11.Text);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("WaitExecutionEntryBars", textBox12.Text);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("UseCommissionInProfit", checkBox13.Checked == true ? "true" : "false");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("Slippage", textBox13.Text);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("SlippagePct", textBox14.Text);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("TakeProfitNoSlippage", checkBox14.Checked == true ? "true" : "false");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("OpenPositionNoSlippage", checkBox15.Checked == true ? "true" : "false");
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("ByMarketAsLimt", checkBox16.Checked == true ? "true" : "false");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("InvalidStopsByMarket", checkBox17.Checked == true ? "true" : "false");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Set_DATA_Json("OrderExpirationDays", textBox15.Text);
        }
    }
}