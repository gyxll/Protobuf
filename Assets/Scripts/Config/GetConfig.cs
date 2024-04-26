using Com.Dxll.Proto;
using System.Collections.Generic;
using Google.Protobuf.Collections;
public class GetConfig{ 
	private ConfigDatabase AllConfig;
	public void InitConfig(ConfigDatabase data){
		AllConfig = data;
	}

	private Dictionary<int, buffConfig> buffDic;
	public buffConfig Getbuff(int id)
{
		if(null == buffDic)
		{
			buffDic = new Dictionary<int, buffConfig>(AllConfig.Buff.Config.Count);
			foreach(buffConfig oneConfig in AllConfig.Buff.Config)
				buffDic[oneConfig.Id] = oneConfig;
		}
		if(buffDic.ContainsKey(id))
			return buffDic[id];
		return null;
	}
	public RepeatedField<buffConfig> GetbuffList(){return AllConfig.Buff.Config;}

	private Dictionary<int, testConfig> testDic;
	public testConfig Gettest(int id)
{
		if(null == testDic)
		{
			testDic = new Dictionary<int, testConfig>(AllConfig.Test.Config.Count);
			foreach(testConfig oneConfig in AllConfig.Test.Config)
				testDic[oneConfig.Id] = oneConfig;
		}
		if(testDic.ContainsKey(id))
			return testDic[id];
		return null;
	}
	public RepeatedField<testConfig> GettestList(){return AllConfig.Test.Config;}
}