using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Service.Tag
{
    public class TagInsertOrUpdateDto
    {

        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 父id
        /// </summary>

        public int? Pid { get; set; }


    }
}
