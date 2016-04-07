using cma.cimiss;
using cma.cimiss.client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Rain1H
{
    class Program
    {
        static string inipath, awspath, logpath;

        static void Main(string[] args)
        {
            //getZfile(@"\\10.203.6.6\begy\umsg\qyzdz\aws_new\20160319\", "");
            test();
            //datafromSQL();
        }
        static void test()
        {

            inipath = System.Environment.CurrentDirectory.Substring(0, System.Environment.CurrentDirectory.LastIndexOf(@"\")) + "\\conf";
            awspath = INIHelper.Read("路径设置", "Micaps雨量路径", Path.Combine(inipath, "aws.ini"));
            logpath = System.Environment.CurrentDirectory.Substring(0, System.Environment.CurrentDirectory.LastIndexOf(@"\")) + "\\log";

            wrtielog(string.Format("读取配置文件Micaps1路径{0}成功.", awspath));

            //循环获取降水
            //DateTime st = DateTime.Parse("2015-05-01 08:00");
            //DateTime et = DateTime.Parse("2015-10-01 08:00");
            //TimeSpan ts = et.Subtract(st).Duration();
            //int num = (int)ts.TotalHours;
            //for (int i = 0; i < num; i++)
            //{


            /* 1. 定义client对象 */
            DataQueryClient client = new DataQueryClient();

            /* 2. 调用方法的参数定义，并赋值 */
            /* 2.1 用户名&密码 */
            string userId = "user_gzqxt";
            string pwd = "user_gzqxt_api1";
            /* 2.2  接口ID */
            string interfaceId = "getSurfEleInRegionByTime";

            /* 2.3  接口参数，多个参数间无顺序 */
            Dictionary<string, string> param = new Dictionary<string, string>();
            //必选参数
            param.Add("dataCode", "SURF_CHN_MUL_HOR"); //资料代码
            //param.Add("elements", "Station_ID_C,PRE_1h,PRS,RHU,VIS,WIN_S_Avg_2mi,WIN_D_Avg_2mi,Q_PRS");//检索要素：站号、站名、小时降水、气压、相对湿度、能见度、2分钟平均风速、2分钟风向
            param.Add("elements", "Station_ID_C,Station_Name,City,Lon,Lat,Alti,PRE_1h");
            param.Add("adminCodes", "520000");
            //时间参量
            DateTime _datetime = DateTime.Now.ToUniversalTime();
            //DateTime _datetime = st.AddHours(i).ToUniversalTime();

            param.Add("times", _datetime.ToString("yyyyMMddHH0000"));
            //param.Add("times", "20151009000000"); //检索时间
            //可选参数
            param.Add("orderby", "Station_ID_C:ASC"); //排序：按照站号从小到大
            //param.Add("limitCnt", "10"); //返回最多记录数：10
            /* 2.4 返回对象 */
            RetArray2D retArray2D = new RetArray2D();

            /* 3. 调用接口 */
            try
            {
                //初始化接口服务连接资源
                wrtielog("初始化Cimiss接口服务连接资源");
                client.initResources();
                //调用接口
                int rst = client.callAPI_to_array2D(userId, pwd, interfaceId, param, retArray2D);
                //输出结果
                if (rst == 0)
                { //正常返回
                    wrtielog(string.Format("接口返回：代码{0}，{1}条数据，耗时{2}ms", retArray2D.request.errorCode, retArray2D.data.Length, retArray2D.request.takeTime));
                    ClibUtil clibUtil = new ClibUtil();
                    clibUtil.outputRst(retArray2D);
                    //获取路径
                    if (!Directory.Exists(awspath))
                        awspath = AppDomain.CurrentDomain.BaseDirectory;
                    clibUtil.outputMicaps1(awspath, _datetime, retArray2D);
                    wrtielog(string.Format("处理{0}写入{1}成功.", awspath, _datetime.ToString("yyMMddHH.000")));
                }
                else
                { //异常返回
                    Console.WriteLine("[error] StaElemSearchAPI_CLIB_callAPI_to_array2D.");
                    Console.Write("\treturn code: {0}. \n", rst);
                    Console.Write("\terror message: {0}.\n", retArray2D.request.errorMessage);
                    wrtielog(retArray2D.request.errorMessage);
                }
            }
            catch (Exception e)
            {
                //异常输出
                Console.WriteLine(e.Message);
                wrtielog(e.Message);
            }
            finally
            {
                //释放接口服务连接资源
                client.destroyResources();
                //Console.ReadKey();
            }
            //}//循环时间
        }

        static void getZfile(string Sourcepath, string zdzPath)
        {
            //IEnumerable<string> list = Directory.GetFiles(Sourcepath).Where(p => File.GetCreationTime(p) > DateTime.Now.AddMinutes(-30));
            IEnumerable<string> list = Directory.GetFiles(Sourcepath, string.Format("Z_SURF_I_*_{0}*_O_AWS_FTM.TXT",DateTime.UtcNow.AddHours(-1).ToString("yyyyMMddHH")));
            foreach (string _file in list)
            {
                if (File.GetCreationTime(_file) > DateTime.Now.AddMinutes(-10))
                    Console.WriteLine(_file);
                //break;
            }

        }

        static void datafromSQL()
        {
            inipath = System.Environment.CurrentDirectory.Substring(0, System.Environment.CurrentDirectory.LastIndexOf(@"\")) + "\\conf";
            awspath = INIHelper.Read("路径设置", "Micaps雨量路径", Path.Combine(inipath, "aws.ini"));
            logpath = System.Environment.CurrentDirectory.Substring(0, System.Environment.CurrentDirectory.LastIndexOf(@"\")) + "\\log";

            wrtielog(string.Format("读取配置文件Micaps1路径{0}成功.", awspath));

            string sqlConnect = @"server=10.203.6.35;database=AWSGZ;user=gzqyz;pwd=gzqyz";
            SqlConnection connect = new SqlConnection(sqlConnect);

            //循环获取降水
            //DateTime st = DateTime.Parse("2015-04-01 08:00");
            //DateTime et = DateTime.Parse("2015-08-01 08:00");
            //TimeSpan ts = et.Subtract(st).Duration();
            //int nt = (int)ts.TotalHours;
            //for (int k = 0; k < nt; k++)
            //{
            //    DateTime _datetime = st.AddHours(k);
            DateTime _datetime = DateTime.Now;

            string search = @"SELECT     dbo.StationPar.StationID, dbo.StationPar.Name, dbo.StationPar.Area, (CONVERT(decimal(10, 2), SUBSTRING(dbo.StationPar.Longitude, 1, 3)) + CONVERT(decimal(10, 2), 
                      SUBSTRING(dbo.StationPar.Longitude, 4, 2)) / 60) + CONVERT(decimal(10, 2), SUBSTRING(dbo.StationPar.Longitude, 6, 2)) / 3600 AS Lon, (CONVERT(decimal(10, 2), 
                      SUBSTRING(dbo.StationPar.Latitude, 1, 2)) + CONVERT(decimal(10, 2), SUBSTRING(dbo.StationPar.Latitude, 3, 2)) / 60) + CONVERT(decimal(10, 2), SUBSTRING(dbo.StationPar.Latitude, 5, 2)) 
                      / 3600 AS Lat, ISNULL(dbo.StationPar.ObserveElevation, 99999) / 10 AS Alti, dbo.tabHourData.R1H / 10
FROM         dbo.StationPar INNER JOIN
                      dbo.tabHourData ON dbo.StationPar.StationID = dbo.tabHourData.StationID
WHERE     (dbo.tabHourData.ObservTime = '" + _datetime.ToString("yyyy-MM-dd HH:00") + "')";


            SqlDataAdapter sa = new SqlDataAdapter(search, connect);
            DataSet ds = new DataSet();
            RetArray2D retArray2D = new RetArray2D();
            retArray2D.request = new RequestInfo();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            try
            {
                sa.Fill(ds);
                int num = ds.Tables[0].Rows.Count;
                int num1 = ds.Tables[0].Columns.Count;
                retArray2D.data = Enumerable.Range(0, num).Select(x => new string[num1]).ToArray();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            retArray2D.data[i][j] = ds.Tables[0].Rows[i][j].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                retArray2D.request.errorMessage = ex.Message;
            }
            retArray2D.request.takeTime = (int)stopwatch.Elapsed.TotalSeconds;

            stopwatch.Stop();

            wrtielog(string.Format("接口返回：{0}，{1}条数据，耗时{2}ms", retArray2D.request.errorMessage, retArray2D.data.Length, retArray2D.request.takeTime));
            ClibUtil clibUtil = new ClibUtil();
            clibUtil.outputRst(retArray2D);
            //获取路径
            if (!Directory.Exists(awspath))
                awspath = AppDomain.CurrentDomain.BaseDirectory;
            clibUtil.outputMicaps1(awspath, _datetime, retArray2D);
            wrtielog(string.Format("处理{0}写入{1}成功.", awspath, _datetime.ToString("yyMMddHH.000")));

            //}//循环结束
            connect.Close();
        }
        /// <param name="degree">度分秒</param>
        /// <param name="l">度分2，度分秒3</param>
        /// <returns>度</returns>
        public static float degreeToFloat(float degree, int l)
        {
            if (l == 2)
            {
                int d = (int)(degree / 100);
                float x = degree % 100;
                float z = x / 60f;
                return d + z;
            }
            else
            {
                int d = (int)(degree / 10000);
                float x = (int)((degree - d * 10000) / 100) / 60f;
                float z = (degree - d * 10000 - x * 100) / 3600f;
                //return d + x + z;
                return d + x;
            }
        }
        static void wrtielog(string log)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(logpath, DateTime.Now.ToString("'Rain1H'yyyyMMdd'.txt'")), true, Encoding.GetEncoding("GB2312"));
            sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + log);
            sw.Close();
        }
    }

    class ClibUtil
    {
        /*
       * 输出返回结果，RetArray2D
       */
        public void outputRst(RetArray2D retArray2D)
        {
            /* 1. 请求信息 */
            Console.WriteLine("请求信息：request，所属类：RequestInfo");
            Console.WriteLine("\t" + "成员：");
            Console.WriteLine("\t" + "returnCode（返回码）：" + retArray2D.request.errorCode);
            Console.WriteLine("\t" + "returnMessage（提示信息）：" + retArray2D.request.errorMessage);
            Console.WriteLine("\t" + "requestElems（检索请求的要素）：" + retArray2D.request.requestElems);
            Console.WriteLine("\t" + "requestParams（检索请求的参数）：" + retArray2D.request.requestParams);
            Console.WriteLine("\t" + "requestTime（请求发起时间）：" + retArray2D.request.requestTime);
            Console.WriteLine("\t" + "responseTime（请求返回时间）：" + retArray2D.request.responseTime);
            Console.WriteLine("\t" + "takeTime（请求耗时，单位ms）：" + retArray2D.request.takeTime);
            Console.WriteLine();

            /* 2. 返回的数据  */
            if (retArray2D.data.Length > 0)
            {
                Console.WriteLine("结果集：retArray2D，所属类：RetArray2D");
                Console.Write("\t成员：data，类：String[][]，值(记录数：{0}，要素数：{1}）：\n", retArray2D.data.Length, retArray2D.data[0].Length);
                //遍历数据：retArray2D.data
                //行数：retArray2D.data.length
                Console.WriteLine("\t---------------------------------------------------------------------");
                for (int iRow = 0; iRow < retArray2D.data.Length; iRow++)
                {
                    Console.Write("\t");
                    //列数： retArray2D.data[iRow].length
                    for (int iCol = 0; iCol < retArray2D.data[iRow].Length; iCol++)
                    {
                        Console.Write(retArray2D.data[iRow][iCol] + "\t");
                    }
                    Console.WriteLine();
                    //DEMO中，最多只输出10行
                    if (iRow > 10)
                    {
                        Console.WriteLine("\t......");
                        break;
                    }
                }
                Console.WriteLine("\t---------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("结果集：retArray2D，所属类：RetArray2D");
                Console.Write("\t成员：data，类：String[][]，值(记录数：{0}）：\n", 0);
            }
        }

        public void outputMicaps1(string filepath, DateTime dt, RetArray2D retArray2D)
        {
            string filename = Path.Combine(filepath, dt.ToString("yyMMddHH.000"));
            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding("GB2312"));
            sw.WriteLine(string.Format("diamond 3 {0}", dt.ToString("yyyy年MM月dd日HH时(小时雨量)")));
            sw.WriteLine(dt.ToString("yyyy MM dd HH -1 0 1 25 0 1 ") + retArray2D.data.Length);
            for (int i = 0; i < retArray2D.data.Length; i++)
            {
                string statid = retArray2D.data[i][0];
                if (statid.IndexOf("R", StringComparison.OrdinalIgnoreCase) >= 0)
                    statid = statid.Replace("R", "82");

                sw.WriteLine(string.Format("{0:000000} {1:f2} {2:f2} {3:f2} {4:f1}", statid, retArray2D.data[i][3], retArray2D.data[i][4], retArray2D.data[i][5], retArray2D.data[i][6]));
            }
            sw.Close();
        }
    }

    /// <summary>
    /// 读写INI文件的类。
    /// </summary>
    public class INIHelper
    {
        // 读写INI文件相关。
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);


        /// <summary>
        /// 向INI写入数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Value">值名。</PARAM>
        public static void Write(string Section, string Key, string Value, string path)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }


        /// <summary>
        /// 读取INI数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Path">值名。</PARAM>
        /// <returns>相应的值。</returns>
        public static string Read(string Section, string Key, string path)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetAllSectionNames(out string[] sections, string path)
        {
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }

        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetAllKeyValues(string section, out string[] keys, out string[] values, string path)
        {
            byte[] b = new byte[65535];

            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            ArrayList result = new ArrayList();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }

            return 0;
        }

    }

}
