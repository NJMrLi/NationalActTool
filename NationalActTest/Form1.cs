using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using StackExchange.Redis;
using static NationalActTest.tcysysactivitysupportdbDataSet;
using NationalActTest;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SummerActTest
{
    public partial class Form1 : Form
    {

        public static string DbName = "TcySysActivitySupportDb_test";
        public string ConnectionString = $"data source=192.168.1.251,1435;initial catalog={DbName};persist security info=True;user id=tcysysactivitysupport;password=jeh74gju";

        public string Host = "192.168.1.209:9079,password=gki4jg8iu8g5g";

        public int db = 15;

        public Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        //清除数据库
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = ConnectionString;
                con.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                //删除数据库数据
                com.CommandText = $"DELETE FROM [{DbName}].[dbo].[NationalDayActAwardSendLogs];" +
                    $"DELETE FROM[{DbName}].[dbo].[NationalDayActSignInLogs];" +
                    $"DELETE FROM[{DbName}].[dbo].[NationalDayOrders];" +
                    $"DELETE FROM[{DbName}].[dbo].[NationalDayPackageStocks];" +
                    $"DELETE FROM[{DbName}].[dbo].[NationalDaySignInActUserChances];" +
                    $"DELETE FROM[{DbName}].[dbo].[NationalDayUserRebates];" +
                    $"DELETE FROM[{DbName}].[dbo].[UserSpecialItemsLogs];" +
                    $"DELETE FROM[{DbName}].[dbo].[UserSpecialItems];";

                SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                dr.Close();//关闭执行
                con.Close();//关闭数据库

                MessageBox.Show("数据库清除完成!");
            }

        }

        //清空缓存
        private void button3_Click(object sender, EventArgs e)
        {
            var date = ToDateInt8(DateTime.Now);
            //取连接对象
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);

            var database = redis.GetDatabase(db);

            var key1 = "TcySys_NationalAct_MyAward_SubActId_1";
            var key2 = "TcySys_NationalAct_MyAward_SubActId_2";
            var key3 = "TcySys_NationalAct_MyAward_SubActId_3";
            var key4 = $"TcySys_NationalAct_SignOnDevNo_{date}";
            var key5 = $"TcySys_NationalAct_CommonIpSignOnCount_{date}";
            var key6 = "TcySys_NationalAct_SignOnAct_UserStatus";
            var key7 = "TcySys_NationalAct_ChargeAct_PackageList";
            var key8 = "TcySys_NationalAct_ChargeAct_PackageStocks";
            for (var i = 1; i < 10; i++)
            {
                var key9 = $"TcySys_NationalAct_ChargeAct_UserBuyList_{i}";
                database.KeyDelete(key9);
            }

            database.KeyDelete(key1);
            database.KeyDelete(key2);
            database.KeyDelete(key3);
            database.KeyDelete(key4);
            database.KeyDelete(key5);
            database.KeyDelete(key6);
            database.KeyDelete(key7);
            database.KeyDelete(key8);
            MessageBox.Show("Redis清除完成！");
        }

        /// <summary>
        /// 活动一 签到数据模拟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initAct1_Click(object sender, EventArgs e)
        {
            try
            {
                var userId = Convert.ToInt32(txtAct1UserId.Text);

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = // "DELETE FROM [{DbName}].[dbo].[NationalDayActSignInLogs] WHERE  UserId = " + userId + ";" +
                        $"UPDATE [{DbName}].[dbo].[NationalDaySignInActUserChances] " +
                        "SET " +
                        "[UserId] =" + userId + "," +
                        "[LastSignOnDateTime] = '" + DateTime.Now.AddDays(-1) +
                        "',[LastSignOnDate] =" + ToDateInt8(DateTime.Now.AddDays(-1)) +
                        " WHERE UserId = " + userId +
                        "And [TotalSignOnCount] > 0 And [TotalSignOnCount] < 8";

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }

                var key1 = "TcySys_NationalAct_SignOnAct_UserStatus";

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key1, userId);

                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }

        }

        /// <summary>
        /// 签到 清除设备IP限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Act1AllComplete_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                var date = ToDateInt8(DateTime.Now);
                var key23 = $"TcySys_NationalAct_SignOnDevNo_{date}";
                var key24 = $"TcySys_NationalAct_CommonIpSignOnCount_{date}";
                database.KeyDelete(key23);
                database.KeyDelete(key24);
                MessageBox.Show("修改完成！");
            }
            catch
            {
                MessageBox.Show("输入有问题");
            }

        }

        public static int ToDateInt8(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var userId = Convert.ToInt32(txtAct1UserId.Text);

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = // "DELETE FROM [{DbName}].[dbo].[NationalDayActSignInLogs] WHERE  UserId = " + userId + ";" +
                        $"Delete From [{DbName}].[dbo].[NationalDaySignInActUserChances] " +
                        " WHERE UserId = " + userId;

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }

                var key1 = "TcySys_NationalAct_SignOnAct_UserStatus";

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key1, userId);

                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet1.NationalDayActAwardSendLogs”中。您可以根据需要移动或删除它。
            this.tcysysactivitysupportdbDataSet1.NationalDayActAwardSendLogs.Clear();
            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet.UserSpecialItems”中。您可以根据需要移动或删除它。
            this.tcysysactivitysupportdbDataSet.UserSpecialItems.Clear();
            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet.NationalDayUserRebates”中。您可以根据需要移动或删除它。
            this.tcysysactivitysupportdbDataSet.NationalDayUserRebates.Clear();

            var userId = Convert.ToInt64(textBox1.Text);
            var dataSet1 = this.tcysysactivitysupportdbDataSet.UserSpecialItems;
            var dataSet2 = this.tcysysactivitysupportdbDataSet.NationalDayUserRebates;
            var dataSet3 = this.tcysysactivitysupportdbDataSet1.NationalDayActAwardSendLogs;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM [{DbName}].[dbo].[UserSpecialItems] where UserId = " + userId;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet1);  //填充数据            
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM [{DbName}].[dbo].[NationalDayUserRebates] where UserId = " + userId;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet2);  //填充数据    
            }

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand command = conn.CreateCommand();
                command.CommandText = $"SELECT * FROM [{DbName}].[dbo].[NationalDayActAwardSendLogs] where UserId = " + userId + "And ActId = 3 ";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet3);  //填充数据    
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet1.NationalDayActAwardSendLogs”中。您可以根据需要移动或删除它。
            //this.nationalDayActAwardSendLogsTableAdapter.Fill(this.tcysysactivitysupportdbDataSet1.NationalDayActAwardSendLogs);
            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet.UserSpecialItems”中。您可以根据需要移动或删除它。
            //this.userSpecialItemsTableAdapter.Fill(this.tcysysactivitysupportdbDataSet.UserSpecialItems);
            // TODO: 这行代码将数据加载到表“tcysysactivitysupportdbDataSet.NationalDayUserRebates”中。您可以根据需要移动或删除它。
            //this.nationalDayUserRebatesTableAdapter.Fill(this.tcysysactivitysupportdbDataSet.NationalDayUserRebates);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(textBox1.Text);
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = // "DELETE FROM [{DbName}].[dbo].[NationalDayActSignInLogs] WHERE  UserId = " + userId + ";" +
                        $"Delete From [{DbName}].[dbo].[NationalDayActAwardSendLogs]  WHERE UserId = " + userId + "And ActId = 3 " +
                        $"Delete From [{DbName}].[dbo].[NationalDayActAwardSendLogs]  WHERE UserId = " + userId +
                        $"Delete From [{DbName}].[dbo].[UserSpecialItems]  WHERE UserId = " + userId +
                        $"Delete From [{DbName}].[dbo].[NationalDayUserRebates]  WHERE UserId = " + userId;

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }

                var key1 = "TcySys_NationalAct_UserConstP";
                var key2 = "TcySys_NationalAct_UserRebateSliverInfo";
                var key3 = "TcySys_NationalAct_MyAward_SubActId_3";
                var key4 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_Dice";
                var key5 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_Dice_Pay";
                var key6 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_DoubleCount";
                var key7 = "TcySys_NationalAct_LotteryCount";
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key1, userId);
                database.HashDelete(key2, userId);
                database.HashDelete(key3, userId);
                database.HashDelete(key4, userId);
                database.HashDelete(key5, userId);
                database.HashDelete(key6, userId);
                database.HashDelete(key7, userId);
                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(ytUserId1.Text);
            var count = Convert.ToInt64(yututxtcount.Text);

            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText =
                        $"Delete From [{DbName}].[dbo].[UserSpecialItems]  WHERE UserId = " + userId + " and SpecialItemId = 41;" +
                        $"Delete From [{DbName}].[dbo].[UserSpecialItemsLogs]  WHERE UserId = " + userId + " and SpecialItemId = 41;" +
                        $"INSERT INTO [{DbName}].[dbo].[UserSpecialItems] ([UserId],[MasterActId],[SpecialItemId],[SpecialItemName],[TotalCount],[UsedCount],[CreationTime],[UpdateTime]" +
                           ",[IsDeleted],[SpecialItemCode]) VALUES(" + userId + ",  90004, 14, '玉兔骰子', " + count + ", 0, '" + DateTime.Now + "', '" + DateTime.Now + "', 0, 'NationalDay_Dice');";

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }


                var key4 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_Dice";
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key4, userId);

                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(textBox4.Text);
            var count = Convert.ToInt64(textBox5.Text);

            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText =
                        $"Delete From [{DbName}].[dbo].[UserSpecialItems]  WHERE UserId = " + userId + " and SpecialItemId = 43;" +
                        $"Delete From [{DbName}].[dbo].[UserSpecialItemsLogs]  WHERE UserId = " + userId + " and SpecialItemId = 43;" +
                        $"INSERT INTO [{DbName}].[dbo].[UserSpecialItems] ([UserId],[MasterActId],[SpecialItemId],[SpecialItemName],[TotalCount],[UsedCount],[CreationTime],[UpdateTime]" +
                           ",[IsDeleted],[SpecialItemCode]) VALUES(" + userId + ", 90004, 15, '玉兔骰子(付费)', " + count + ", 0, '" + DateTime.Now + "', '" + DateTime.Now + "', 0, 'NationalDay_Dice_Pay');";

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }


                var key4 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_Dice_Pay";
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key4, userId);

                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(textBox6.Text);
            var count = Convert.ToInt64(textBox7.Text);

            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText =
                        $"Delete From [{DbName}].[dbo].[UserSpecialItems]  WHERE UserId = " + userId + " and SpecialItemId = 42;" +
                        $"Delete From [{DbName}].[dbo].[UserSpecialItemsLogs]  WHERE UserId = " + userId + " and SpecialItemId = 42;" +
                        $"INSERT INTO [{DbName}].[dbo].[UserSpecialItems] ([UserId],[MasterActId],[SpecialItemId],[SpecialItemName],[TotalCount],[UsedCount],[CreationTime],[UpdateTime]" +
                           ",[IsDeleted],[SpecialItemCode]) VALUES(" + userId + ", 90004, 16, '加倍次数', " + count + ", 0, '" + DateTime.Now + "', '" + DateTime.Now + "', 0, 'NationalDay_DoubleCount');";

                    SqlDataReader dr = com.ExecuteReader();//执行SQL语句

                    dr.Close();//关闭执行
                    con.Close();//关闭数据库
                }


                var key4 = "TcySys_Activity_UserSpecialItems_ItemCode_NationalDay_DoubleCount";
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                var database = redis.GetDatabase(db);
                database.HashDelete(key4, userId);

                MessageBox.Show("修改完成！");

            }
            catch
            {
                MessageBox.Show("输入有问题");
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            var key1 = "TcySys_SpecialActConfig_Master_90004_Sub_3";
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
            var database = redis.GetDatabase(db);
            var configDetial = database.StringGet(key1);
            var config = JsonConvert.DeserializeObject<NationalDayMonopolyActConfig>(configDetial);

            pcFreeGuid.Text = config.PcFreeGuid;
            PcFreeDoubleGuid.Text = config.PcFreeDoubleGuid;
            pcPayGuid.Text = config.PcPayGuid;
            pcPayDoubleGuid.Text = config.PcPayDoubleGuid;

            mobileFreeGuid.Text = config.MobileFreeGuid;
            mobilePayGuid.Text = config.MobilePayGuid;
            mobileFreeDoubleGuid.Text = config.MobileFreeDoubleGuid;
            mobilePayDoubleGuid.Text = config.MobilePayDoubleGuid;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var count = 10000;
            var list = new List<Double>();

            var award1 = Convert.ToDouble(t1.Text) / 100;
            var returnNum = ReturnAwardValue(award1, count, 1, 0);
            var award2 = Convert.ToDouble(t2.Text) / 100;
            returnNum = ReturnAwardValue(award2, count, 2, returnNum);
            var award3 = Convert.ToDouble(t3.Text) / 100;
            returnNum = ReturnAwardValue(award3, count, 3, returnNum);
            var award4 = Convert.ToDouble(t4.Text) / 100;
            returnNum = ReturnAwardValue(award4, count, 4, returnNum);
            var award5 = Convert.ToDouble(t5.Text) / 100;
            returnNum = ReturnAwardValue(award5, count, 5, returnNum);
            var award6 = Convert.ToDouble(t6.Text) / 100;
            returnNum = ReturnAwardValue(award6, count, 6, returnNum);
            var award7 = Convert.ToDouble(t7.Text) / 100;
            returnNum = ReturnAwardValue(award7, count, 7, returnNum);
            var award8 = Convert.ToDouble(t8.Text) / 100;
            returnNum = ReturnAwardValue(award8, count, 8, returnNum);
            var award9 = Convert.ToDouble(t9.Text) / 100;
            returnNum = ReturnAwardValue(award9, count, 9, returnNum);
            var award10 = Convert.ToDouble(t10.Text) / 100;
            returnNum = ReturnAwardValue(award10, count, 10, returnNum);
            var award11= Convert.ToDouble(t11.Text) / 100;
            returnNum = ReturnAwardValue(award11, count, 11, returnNum);
            var award12= Convert.ToDouble(t12.Text) / 100;
            returnNum = ReturnAwardValue(award12, count, 12, returnNum);
        }

        private double ReturnAwardValue(double award,int count,int num,double startCount)
        {
            if (award == 0)
            {
                return startCount;
            }

            double returnCount = award * count + 1 + startCount;

            switch (num)
            {
                case 1:
                    l1.Text = returnCount.ToString();
                    return returnCount - 1;
                case 2:
                    l2.Text = returnCount.ToString();
                    return returnCount - 1;
                case 3:
                    l3.Text = returnCount.ToString();
                    return returnCount - 1;
                case 4:
                    l4.Text = returnCount.ToString();
                    return returnCount - 1;
                case 5:
                    l5.Text = returnCount.ToString();
                    return returnCount - 1;
                case 6:
                    l6.Text = returnCount.ToString();
                    return returnCount - 1;
                case 7:
                    l7.Text = returnCount.ToString();
                    return returnCount - 1;
                case 8:
                    l8.Text = returnCount.ToString();
                    return returnCount - 1;
                case 9:
                    l9.Text = returnCount.ToString();
                    return returnCount - 1;
                case 10:
                    l10.Text = returnCount.ToString();
                    return returnCount - 1;
                case 11:
                    l11.Text = returnCount.ToString();
                    return returnCount - 1;
                case 12:
                    l12.Text = returnCount.ToString();
                    return returnCount - 1;
                default:
                    return 0;
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            var stringBuilder = new StringBuilder();
            var stringBuilder2 = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                var index = GeneratePrizeIndex(i);
                Thread.Sleep(5);
                stringBuilder.Append(index + " ");              
            }

            for (var i = 0; i < 10; i++)
            {
                var index = GeneratePrizeIndex2(i);
                stringBuilder2.Append(index + " ");
            }

            textBox2.Text = stringBuilder.ToString();
            textBox3.Text = stringBuilder2.ToString();
        }

        /// <summary>
        /// 随机产生奖励的位置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int GeneratePrizeIndex(int i)
        {      
            var random = new Random();            
            var weight = random.Next(1, 10001);
            return weight;
        }

        /// <summary>
        /// 随机产生奖励的位置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int GeneratePrizeIndex2(int i)
        {
            //var seed = (unchecked((int)DateTime.Now.Ticks % 100 + i * 1234));
            //var random = new Random(seed);
            var weight = random.Next(1, 10001);
            return weight;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            button12.Enabled = false;
            progressBar1.Value = 0;
            lCount.Text = 0.ToString();
            CheckForIllegalCrossThreadCalls = false;
            var countDic = new Dictionary<int, int>();
            countDic[1] = 0;
            countDic[2] = 0;        
            countDic[3] = 0;
            countDic[4] = 0;
            countDic[5] = 0;
            countDic[6] = 0;
            countDic[7] = 0;
            countDic[8] = 0;
            countDic[9] = 0;
            countDic[10] = 0;
            countDic[11] = 0;
            countDic[12] = 0;

            lbl1.Text = string.Empty;
            lbl2.Text = string.Empty;
            lbl3.Text = string.Empty;
            lbl4.Text = string.Empty;
            lbl5.Text = string.Empty;
            lbl6.Text = string.Empty;
            lbl7.Text = string.Empty;
            lbl8.Text = string.Empty;
            lbl9.Text = string.Empty;
            lbl10.Text = string.Empty;
            lbl11.Text = string.Empty;
            lbl12.Text = string.Empty;

            try
            {
                Task.Run(() =>
                {
                    var key1 = "TcySys_SpecialActConfig_Master_90004_Sub_3";
                    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Host);
                    var database = redis.GetDatabase(db);
                    var configDetial = database.StringGet(key1);
                    var config = JsonConvert.DeserializeObject<NationalDayMonopolyActConfig>(configDetial);
                    var lotteryCount = Convert.ToDouble(textBox8.Text);
                    var type = 0;
                    try
                    {
                        type = Convert.ToInt16(comboBox1.Text.Split('.')[0]);
                    }
                    catch
                    {
                        MessageBox.Show("请选择类型");
                    }

                    var j = 1;

                    switch (type)
                    {
                        case 1:
                            GetPrecent(countDic, lotteryCount, config.FreeAwardConfig); break;
                        case 2:
                            GetPrecent(countDic, lotteryCount, config.DoubleAwardConfig); break;
                        case 3:
                            GetPrecent(countDic, lotteryCount, config.PayAwardConfig); break;
                        case 4:
                            GetPrecent(countDic, lotteryCount, config.DoublePayAwardConfig); break;
                        default:
                            break;
                    }

                    lbl1.Text = (countDic[1] / lotteryCount * 100.00).ToString();
                    lbl2.Text = (countDic[2] / lotteryCount * 100.00).ToString();
                    lbl3.Text = (countDic[3] / lotteryCount * 100.00).ToString();
                    lbl4.Text = (countDic[4] / lotteryCount * 100.00).ToString();
                    lbl5.Text = (countDic[5] / lotteryCount * 100.00).ToString();
                    lbl6.Text = (countDic[6] / lotteryCount * 100.00).ToString();
                    lbl7.Text = (countDic[7] / lotteryCount * 100.00).ToString();
                    lbl8.Text = (countDic[8] / lotteryCount * 100.00).ToString();
                    lbl9.Text = (countDic[9] / lotteryCount * 100.00).ToString();
                    lbl10.Text = (countDic[10] / lotteryCount * 100.00).ToString();
                    lbl11.Text = (countDic[11] / lotteryCount * 100.00).ToString();
                    lbl12.Text = (countDic[12] / lotteryCount * 100.00).ToString();
                    progressBar1.Value = 100;


                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                button12.Enabled = true;
            }
 

 
        }

        public void GetPrecent(Dictionary<int, int> dic,double lotteryCount, List<AwardConfigItem> config)
        {
            var step = lotteryCount / 100;
            var progressCount = 1;
            var j = 1;
            for (var i = 1; i <= lotteryCount; i++)
            {
                lCount.Text = i.ToString();
                j = j % 10;
                var index = GeneratePrizeIndex2(j);
                foreach (var item in config)
                {
                    if (index < item.Procent)
                    {
                        dic[item.AwardIndex]++;
                        break;
                    }
                }

                if (i % 200 == 0)
                {                  
                    Thread.Sleep(1000);
                }

                if (i == step * progressCount)
                {
                    progressBar1.Value++;
                    progressCount++;
                }

                j++;
         
            }
        }

        private void label44_Click(object sender, EventArgs e)
        {

        }
    }
}
