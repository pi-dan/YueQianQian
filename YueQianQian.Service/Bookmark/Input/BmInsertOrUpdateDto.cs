using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Service.Bookmark
{
    public class BmInsertOrUpdateDto
    {
        public string Title { get; set; }

        public string Url { get; set; }


        public int? UserId { get; set; }

        public int TagId { get; set; }
        public string Description { get; set; }
        public int? id { get; set; }

    }
}
