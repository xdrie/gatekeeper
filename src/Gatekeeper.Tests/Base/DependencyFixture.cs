﻿#region

 using System;
 using Gatekeeper.Config;
 using Gatekeeper.Services.Database;
 using Microsoft.EntityFrameworkCore;

 #endregion

namespace Gatekeeper.Tests.Base {
    public class DependencyFixture : IDisposable {
        public SContext serverContext { get; }

        public DependencyFixture() {
            var config = defaultConfig();
            serverContext = new SContext(config);
            using (var db = new AppDbContextFactory(serverContext).create()) {
                db.Database.Migrate();
            }
        }

        public static SConfig defaultConfig() {
            var config = new SConfig();
            config.server.database = null; // use transient database mode
            return config;
        }

        public virtual void Dispose() {
            serverContext.Dispose();
        }
    }
}