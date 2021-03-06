//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://www.freesql.net
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace YueQianQian.Model
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "tag")]
    public partial class TagEntity
    {

        [JsonProperty, Column(Name = "id", IsPrimary = true, IsIdentity = true)]
        public int Id { get; set; }

        [JsonProperty, Column(Name = "last_use", DbType = "datetime")]
        public DateTime? LastUse { get; set; }

        [JsonProperty, Column(Name = "name", DbType = "varchar(32)")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 父id
        /// </summary>
        [JsonProperty, Column(Name = "pid")]
        public int? Pid { get; set; }


        [JsonProperty, Column(Name = "sort", DbType = "tinyint(11)")]
        public int? Sort { get; set; }

        /// <summary>
		/// M:书签目录，N笔记目录
		/// </summary>
		[JsonProperty, Column(Name = "type", DbType = "varchar(5)")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty, Column(Name = "user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 导航属性  父目录
        /// </summary>
        [Navigate(nameof(Pid))]
        public TagEntity FatherEntity { get; set; }

    }

}
