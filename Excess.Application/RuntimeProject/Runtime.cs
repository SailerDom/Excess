﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excess.RuntimeProject
{
    public enum NotificationKind
    {
        Started,
        System,
        Application,
        Error,
        Finished,
    }

    public class Notification
    {
        public NotificationKind Kind { get; set; }
        public string Message { get; set; }
    }

    public interface IRuntimeProject
    {
        bool busy();
        void compile();
        void run();
        void add(string file, int id, string contents);
        void modify(string file, string contents);
        IEnumerable<Notification> notifications();
        string defaultFile();
        string fileContents(string file);
        int fileId(string file);
    }
}
