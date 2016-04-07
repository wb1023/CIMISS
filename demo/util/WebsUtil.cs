using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Services;
using System.Web.Services.Description;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Xml.Serialization;

using System.Diagnostics;
using System.Web.Services.Protocols;
using System.ComponentModel;


namespace cma.cimiss.demo.util
{

    class WebsUtil
    {
        /*
        * TODO:wsdl地址，依具体环境设置
        */
        //private static String wsdl = "http://10.20.76.32:8008/cimiss-web/services/ws?wsdl";
        //private static String wsdl = "http://10.20.76.55/cimiss-web/services/ws?wsdl";

        private static String serverIp = "10.20.76.55";
        /*
         * web service请求服务，获取数据（返回序列化字符串）
         */
        public void outputRst(String[][] data)
        {
            if (data == null)
            {
                Console.WriteLine("返回结果为空");
                return;
            }
            if (data.Length < 1)
            {
                return;
            }
            //第1行为各字段名
            for (int iCol = 0; iCol < data[0].Length; iCol++)
            {
                Console.Write(data[0][iCol] + "\t");
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            //第2行开始为要素值
            for (int iRow = 1; iRow < data.Length; iRow++)
            {
                for (int iCol = 0; iCol < data[iRow].Length; iCol++)
                {
                    Console.Write(data[iRow][iCol] + "\t");
                }
                Console.WriteLine();
                //DEMO中，最多只输出10行
                if (iRow > 10)
                {
                    Console.WriteLine("......");
                    break;
                }
            }
        }
        /*
         * web service请求服务，获取数据（返回序列化字符串）
         */
        public String getWsString(String method1, String @params)
        {
            int type = 1;
            return (String)getWsData(method1, @params, type);
        }
        /*
        * web service请求服务，获取数据（返回二维数组）
         */
        public String[][] getWsArray(String method1, String @params)
        {
            int type = 2;
            String[][] data = (String[][])getWsData(method1, @params, type);
            return data;
        }

        /*
         * 得到数据
         */
        private Object getWsData(String method, String @params, int returnTypes)
        {
            //动态加载,加载dll较直接调用代码耗时
            /** NmicWeb web = new NmicWeb();
            ** return web.getWebData(method, @params, returnTypes);
            ***/

            //---用已生成的代码调用
            Ws ws = new Ws(serverIp);
            if (returnTypes == 1)
            {
                return ws.callAPI_to_serializedStr(@params);
            }
            else if (returnTypes == 2)
            {
                ArrayOfString[] arrayofString = ws.callAPI_to_Array(@params);
                String[][] datas = null;
                int num = arrayofString.Length;
                datas = new string[num][];
                num = 0;
                foreach (ArrayOfString obj in arrayofString)
                {
                    datas[num] = obj.array;
                    num++;
                }
                return datas;
            }
            return null;
        }

    }
}
