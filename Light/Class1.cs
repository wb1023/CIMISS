using cma.cimiss;
using cma.cimiss.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cma.cimiss.demo.util;

namespace Light
{
    public class Class1
    {
        static void Main()
        {
            /* 1. 定义client对象 */
            DataQueryClient client = new DataQueryClient();

            /* 2. 调用方法的参数定义，并赋值 */
            /* 2.1 用户名&密码 */
            string userId = "user_gzqxt";
            string pwd = "user_gzqxt_api1";
            /* 2.2  接口ID */
            string interfaceId = "getUparLightEleInRectByTimeRange";

            /* 2.3  接口参数，多个参数间无顺序 */
            Dictionary<string, string> param = new Dictionary<string, string>();
            //必选参数
            param.Add("dataCode", "UPAR_CHN_LIGHT_MUL"); //资料代码
            //param.Add("elements", "Station_ID_C,PRE_1h,PRS,RHU,VIS,WIN_S_Avg_2mi,WIN_D_Avg_2mi,Q_PRS");//检索要素：站号、站名、小时降水、气压、相对湿度、能见度、2分钟平均风速、2分钟风向
            param.Add("elements", "Datetime,Lat,Lon,Year,Mon,Day,Hour,Min,Second,MSecond,Layer_Num,Lit_Current,MARS_3,Pois_Err,Pois_Type,Lit_Prov,Lit_City,Lit_Cnty");

            //时间参量
            DateTime _datetime = DateTime.Now.ToUniversalTime();
            //DateTime _datetime = st.AddHours(i).ToUniversalTime();

            param.Add("timeRange",string.Format("[{0},{1}]",_datetime.AddHours(-24).ToString("yyyyMMddHH0000"), _datetime.ToString("yyyyMMddHH0000")));
            param.Add("minLat", "24");
            param.Add("maxLat", "30");
            param.Add("minLon", "101");
            param.Add("maxLon", "110");
            //param.Add("times", "20151009000000"); //检索时间
            //可选参数
            //param.Add("orderby", "Station_ID_C:ASC"); //排序：按照站号从小到大
            //param.Add("limitCnt", "10"); //返回最多记录数：10
            /* 2.4 返回对象 */
            RetArray2D retArray2D = new RetArray2D();

            /* 3. 调用接口 */
            try
            {
                //初始化接口服务连接资源
                client.initResources();
                //调用接口
                int rst = client.callAPI_to_array2D(userId, pwd, interfaceId, param, retArray2D);
                //输出结果
                if (rst == 0)
                { //正常返回
                    ClibUtil cu = new ClibUtil();
                    cu.outputRst(retArray2D);
                }
                else
                { //异常返回
                    Console.WriteLine("[error] StaElemSearchAPI_CLIB_callAPI_to_array2D.");
                    Console.Write("\treturn code: {0}. \n", rst);
                    Console.Write("\terror message: {0}.\n", retArray2D.request.errorMessage);
                }
            }
            catch (Exception e)
            {
                //异常输出
                Console.WriteLine(e.Message);
            }
            finally
            {
                //释放接口服务连接资源
                client.destroyResources();
                //Console.ReadKey();
            }
            Console.ReadKey();
        }
    }

}
