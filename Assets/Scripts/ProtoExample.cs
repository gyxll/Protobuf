using Com.Dxll.Proto;
using Google.Protobuf;
using ProtoTest;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProtoExample : MonoBehaviour
{
    GetConfig mGetCfg;
    // Start is called before the first frame update
    void Start()
    {
        MsgResult parseResult = (MsgResult)MsgResult.Descriptor.Parser.ParseJson("{ \"code\": -999, \"errMsg\": \"Error\", \"personValue\": { \"name\": \"NNN\", \"age\": 1, \"phoneNumbers\": [ \"185\" ] } }");
        Person person = new() {
            Age = 1,
            Name = "NNN"
        };
        person.PhoneNumbers.Add("185");
        MsgResult result = new()
        {
            Code = -999,
            ErrMsg = "Error",
            PersonValue = person
        };

        byte[] s = result.ToByteArray();

        MsgResult responseMsgResult = (MsgResult)MsgResult.Descriptor.Parser.ParseFrom(s);
        Debug.Log(responseMsgResult.Code);
        Debug.Log(responseMsgResult.ErrMsg);
        Debug.Log(result.ToString());


        string resName = "Config/ConfigDatabase";
        TextAsset ta = Resources.Load<TextAsset>(resName);
        if (null != ta)
        {
            //初始化所有配置
            ConfigDatabase data = (ConfigDatabase)ConfigDatabase.Descriptor.Parser.ParseFrom(ta.bytes);
            mGetCfg = new GetConfig();
            mGetCfg.InitConfig(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
