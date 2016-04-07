using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using System.Configuration;
using System.Web;
using System.Web.Services.Protocols;

using System.Web.Services;
using System.Net;
using System.IO;

using System.Web.Services.Description;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
namespace cma.cimiss.demo.util
{
    /**
     * xuyj 20141125
     **/
    class NmicWeb
    {
        /*
        * TODO:wsdl地址，依具体环境设置
        */
       private static String wsdl = "http://10.20.76.32:8008/cimiss-web/services/ws?wsdl";
        //命名空间
        private static String targetNamespace = "ws";
       
        bool init()
        {
            bool flag = false;
            // 1. 使用WebClient 下载WSDL 信息。
            WebClient web = new WebClient();
            Stream stream = web.OpenRead(wsdl);
            // 2. 创建和格式化WSDL 文档。
            ServiceDescription description = ServiceDescription.Read(stream);
            // 3. 创建客户端代理代理类。
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap"; // 指定访问协议。
            importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.AddServiceDescription(description, null, null); // 添加WSDL 文档。
            // 4. 使用CodeDom 编译客户端代理类。
            CodeNamespace nmspace = new CodeNamespace(); //为代理类添加命名空间，缺省为全局空间。
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);

            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            //// 5. 保存源代码到文件,然后加载cs文件(可直接调用源码)。当然，你也可以直接保存到内存字符串中。
            //TextWriter writer = File.CreateText("test.cs"); // 指定你所需的源代码文件名。
            //provider.GenerateCodeFromCompileUnit(unit, writer, null);
            //writer.Flush();
            //writer.Close();

            //5. 输出dll
            CompilerParameters parameter = new CompilerParameters();

            parameter.GenerateExecutable = false;

            parameter.GenerateInMemory = true;
            parameter.OutputAssembly = "Nmic.Cimiss.Web.dll";

            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");

            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            // 6. 编译是否正常
            if (!result.Errors.HasErrors)
            {
                flag = true;
            }
            else
            {
                Console.WriteLine(result.Output.ToString());
                flag = false;
            }
            return flag;
        }

        public object getWebData(String wedMethod, String @params, int returnType)
        {
            String buffer = "";
            String[][] bufferArray;

            //  使用Reflection 调用WebService
            Assembly asm = null;
            try
            {
                asm = Assembly.LoadFrom("nmicService.dll");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (asm == null)
            {
                init();
                asm = Assembly.LoadFrom("Nmic.Cimiss.Web.dll");
            }
  
            Type t = asm.GetType(targetNamespace);
            object o = Activator.CreateInstance(t);
            //加载方法
            MethodInfo method = t.GetMethod(wedMethod);
            
            String[] pars = new String[] { @params };
            if (returnType == 1)
            {
                buffer = (String)method.Invoke(o, pars);
                return buffer;
            }
            else if (returnType == 2)
            {
                Object[] results = (Object[])method.Invoke(o, pars);
                //获取ArrayOfString类
                Type t1=asm.GetType("ArrayOfString");
                object o2 = Activator.CreateInstance(t1);
 
                //获取属性
                PropertyInfo pty = t1.GetProperty("array");
                string[] vv = null;
                int num = 0;
                num=results.Length;
                bufferArray = new String[num][];
                num = 0;
                foreach (object obj in results)
                {
                    object array=pty.GetValue(obj, null);
                    vv = (String[])array;
                    bufferArray[num] = vv;
                    num++;
                }
                return bufferArray;
            }
            //Console.WriteLine(method.Invoke(o, null));

            return null;

        }
    }
   
}
