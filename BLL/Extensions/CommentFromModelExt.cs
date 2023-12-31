﻿using BLL.Models.Comments;
using BLL.Models.Posts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class CommentFromModelExt
    {
        public static Comment Convert(this Comment comment, EditCommentViewModel commenteditvm)
        {
            comment.Content = commenteditvm.Content ?? comment.Content;
            return comment;
        }
    }
}
