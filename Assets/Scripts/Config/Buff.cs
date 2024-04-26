// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: buff.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Dxll.Proto {

  /// <summary>Holder for reflection information generated from buff.proto</summary>
  public static partial class BuffReflection {

    #region Descriptor
    /// <summary>File descriptor for buff.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BuffReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpidWZmLnByb3RvEg5Db20uRHhsbC5Qcm90byI/CgpidWZmQ29uZmlnEgoK",
            "AmlkGAEgASgFEhEKCWJ1ZmZfbmFtZRgCIAEoCRISCgpidWZmX3ZhbHVlGAMg",
            "ASgJIjwKDmJ1ZmZDb25maWdEYXRhEioKBkNvbmZpZxgBIAMoCzIaLkNvbS5E",
            "eGxsLlByb3RvLmJ1ZmZDb25maWdiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Dxll.Proto.buffConfig), global::Com.Dxll.Proto.buffConfig.Parser, new[]{ "Id", "BuffName", "BuffValue" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Dxll.Proto.buffConfigData), global::Com.Dxll.Proto.buffConfigData.Parser, new[]{ "Config" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class buffConfig : pb::IMessage<buffConfig> {
    private static readonly pb::MessageParser<buffConfig> _parser = new pb::MessageParser<buffConfig>(() => new buffConfig());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<buffConfig> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Dxll.Proto.BuffReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfig() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfig(buffConfig other) : this() {
      id_ = other.id_;
      buffName_ = other.buffName_;
      buffValue_ = other.buffValue_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfig Clone() {
      return new buffConfig(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    /// <summary>
    ///主键id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "buff_name" field.</summary>
    public const int BuffNameFieldNumber = 2;
    private string buffName_ = "";
    /// <summary>
    ///buff添加属性
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string BuffName {
      get { return buffName_; }
      set {
        buffName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "buff_value" field.</summary>
    public const int BuffValueFieldNumber = 3;
    private string buffValue_ = "";
    /// <summary>
    ///buff添加的属性值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string BuffValue {
      get { return buffValue_; }
      set {
        buffValue_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as buffConfig);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(buffConfig other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (BuffName != other.BuffName) return false;
      if (BuffValue != other.BuffValue) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (BuffName.Length != 0) hash ^= BuffName.GetHashCode();
      if (BuffValue.Length != 0) hash ^= BuffValue.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (BuffName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(BuffName);
      }
      if (BuffValue.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(BuffValue);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (BuffName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BuffName);
      }
      if (BuffValue.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BuffValue);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(buffConfig other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.BuffName.Length != 0) {
        BuffName = other.BuffName;
      }
      if (other.BuffValue.Length != 0) {
        BuffValue = other.BuffValue;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            BuffName = input.ReadString();
            break;
          }
          case 26: {
            BuffValue = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class buffConfigData : pb::IMessage<buffConfigData> {
    private static readonly pb::MessageParser<buffConfigData> _parser = new pb::MessageParser<buffConfigData>(() => new buffConfigData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<buffConfigData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Dxll.Proto.BuffReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfigData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfigData(buffConfigData other) : this() {
      config_ = other.config_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public buffConfigData Clone() {
      return new buffConfigData(this);
    }

    /// <summary>Field number for the "Config" field.</summary>
    public const int ConfigFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Com.Dxll.Proto.buffConfig> _repeated_config_codec
        = pb::FieldCodec.ForMessage(10, global::Com.Dxll.Proto.buffConfig.Parser);
    private readonly pbc::RepeatedField<global::Com.Dxll.Proto.buffConfig> config_ = new pbc::RepeatedField<global::Com.Dxll.Proto.buffConfig>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Dxll.Proto.buffConfig> Config {
      get { return config_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as buffConfigData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(buffConfigData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!config_.Equals(other.config_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= config_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      config_.WriteTo(output, _repeated_config_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += config_.CalculateSize(_repeated_config_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(buffConfigData other) {
      if (other == null) {
        return;
      }
      config_.Add(other.config_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            config_.AddEntriesFrom(input, _repeated_config_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
