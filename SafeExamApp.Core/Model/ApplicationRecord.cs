﻿using System;

namespace SafeExamApp.Core.Model {
    public class ApplicationRecord : SessionRecord {
        public string ApplicationName { get; }

        public ApplicationRecord(DateTime dt, string name) : base(dt) {
            ApplicationName = name;
        }
    }
}
