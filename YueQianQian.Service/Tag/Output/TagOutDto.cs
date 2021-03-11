
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace YueQianQian.Service.Tag
{

    public partial class TagOutDto
    {


        public int Id { get; set; }


        public DateTime? LastUse { get; set; }


        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 父id
        /// </summary>

        public int? Pid { get; set; }



        //public sbyte? Sort { get; set; }

        /// <summary>
        /// M:书签目录，N笔记目录
        /// </summary>

        // public string Type { get; set; } = string.Empty;


        public int UserId { get; set; }
        /// <summary>
        /// 子目录和该目录下的书签总和
        /// </summary>
        public int SonCount { get; set; } = 0;

    }

}
