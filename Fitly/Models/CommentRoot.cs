﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitly.Models
{
    public class CommentRoot
    {
        public ObservableCollection<Comment> Comments { get; set; }
    }
}
