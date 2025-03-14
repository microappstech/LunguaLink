﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Models
{
    public class GroupTeacher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Groups Group { get; set; }
        public int GroupId { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
    }
}
