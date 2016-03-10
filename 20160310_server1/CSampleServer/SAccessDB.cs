using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IO;

namespace CSampleServer
{
    class SAccessDB
    {
        static DataTable dt;
        static DataTable dt2;
        static DataSet ds;
        static DataTable CloneDT;
        static StringBuilder sb;
        static StringWriter stream;

        static public void odpconn(string strcmd)
        {
            string strConn = "User Id=scott;Password=tiger;Data Source=ORCL";

            OracleDataAdapter oraDA = new OracleDataAdapter(strcmd, strConn);
            dt = new DataTable();
            oraDA.Fill(dt);
        }
        /// <summary>
        /// 전송할 dt(sting) 만들기
        ///dt 테이블은 dataset 으로 변환한 후에 xml에 데이터를 저장한후
        ///xml.string()으로 다시 response를 해줌.
        /// </summary>
        /// <param name="dt"></param>
        /// dt 를 변수로 넣고 싶은데.. 메쏘드를 두개로 나누는 것이 나을지
        /// 아니면 public static 변수로 두고 같이 쉐어하는 것이 나을지 의논 ..ㅜㅜ
        static public string makexml()
        {
            ds = new DataSet("XMLTABLE");

            CloneDT = dt.Copy();
            CloneDT.TableName = "XMLTABLE";
            ds.Tables.Add(CloneDT);

            sb = new StringBuilder();
            stream = new StringWriter(sb);
            ds.WriteXml(stream, XmlWriteMode.WriteSchema);

            return stream.ToString();
        }
    }
}
