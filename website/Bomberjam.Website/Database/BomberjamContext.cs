﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        private static readonly DbUser UserAskaiser = CreateInitialUser(Constants.UserAskaiserId, 14242083, "Askaiser", "simmon.anthony@gmail.com");
        private static readonly DbUser UserFalgar = CreateInitialUser(Constants.UserFalgarId, 36072624, "Falgar", "falgar@gmail.com");
        private static readonly DbUser UserXenure = CreateInitialUser(Constants.UserXenureId, 9208753, "Xenure", "xenure@gmail.com");
        private static readonly DbUser UserMinty = CreateInitialUser(Constants.UserMintyId, 26142591, "Minty", "minty@gmail.com");
        private static readonly DbUser UserKalmera = CreateInitialUser(Constants.UserKalmeraId, 5122918, "Kalmera", "kalmera@gmail.com");
        private static readonly DbUser UserPandarf = CreateInitialUser(Constants.UserPandarfId, 1035273, "Pandarf", "pandarf@gmail.com");
        private static readonly DbUser UserMire = CreateInitialUser(Constants.UserMireId, 5489330, "Mire", "mire@gmail.com");

        public BomberjamContext(DbContextOptions<BomberjamContext> options)
            : base(options)
        {
            this.ChangeTracker.Tracked += ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged += ChangeTrackerOnStateChanged;
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbGame> Games { get; set; }
        public DbSet<DbGameUser> GameUsers { get; set; }
        public DbSet<DbQueuedTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbGameUser>().HasKey(x => new { GameID = x.GameId, UserID = x.UserId });

            modelBuilder.Entity<DbUser>().HasIndex(x => x.GithubId).IsUnique();
            modelBuilder.Entity<DbUser>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<DbUser>().HasIndex(x => x.UserName).IsUnique();

            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Status);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Type);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Type).HasConversion<int>();
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Status).HasConversion<int>();

            modelBuilder.Entity<DbUser>().HasData(UserAskaiser, UserFalgar, UserXenure, UserMinty, UserKalmera, UserPandarf, UserMire);

            modelBuilder.Entity<DbQueuedTask>().HasData(
                CreateInitialCompileTask(Guid.NewGuid(), UserAskaiser.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserFalgar.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserXenure.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserMinty.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserKalmera.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserPandarf.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserMire.Id));

            var rng = new Random(42);
            var users = new List<DbUser> { UserAskaiser, UserFalgar, UserXenure, UserMinty, UserKalmera, UserPandarf, UserMire };

            for (var i = 0; i < 4; i++)
            {
                users.Shuffle(rng);
                modelBuilder.Entity<DbQueuedTask>().HasData(CreateInitialGameTask(Guid.NewGuid(), users[0], users[1], users[2], users[3]));
            }
        }

        private static void ChangeTrackerOnTracked(object sender, EntityTrackedEventArgs e)
        {
            if (e.FromQuery)
                return;

            UpdateTimestampableObjects(e.Entry);
        }

        private static void ChangeTrackerOnStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            UpdateTimestampableObjects(e.Entry);
        }

        private static void UpdateTimestampableObjects(EntityEntry entry)
        {
            if (entry.Entity is ITimestampable timestampable)
                UpdateTimestampableObjects(entry.State, timestampable);
        }

        private static void UpdateTimestampableObjects(EntityState state, ITimestampable timestampable)
        {
            if (state == EntityState.Added)
            {
                var utcNow = DateTime.UtcNow;
                timestampable.Created = utcNow;
                timestampable.Updated = utcNow;
            }
            else if (state == EntityState.Modified)
            {
                timestampable.Updated = DateTime.UtcNow;
            }
        }

        public override void Dispose()
        {
            this.ChangeTracker.Tracked -= ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged -= ChangeTrackerOnStateChanged;
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            this.ChangeTracker.Tracked -= ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged -= ChangeTrackerOnStateChanged;
            return base.DisposeAsync();
        }

        private static DbUser CreateInitialUser(Guid id, int githubId, string username, string email) => new DbUser
        {
            Id = id,
            GithubId = githubId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            UserName = username,
            Email = email,
            SubmitCount = 1,
            GameCount = 0,
            IsCompiling = false,
            IsCompiled = false,
            CompilationErrors = string.Empty,
            BotLanguage = string.Empty,
            Points = Constants.InitialPoints
        };

        private static DbQueuedTask CreateInitialCompileTask(Guid taskId, Guid userId) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Compile,
            Data = userId.ToString("D"),
            UserId = userId,
            Status = QueuedTaskStatus.Created,
        };

        private static DbQueuedTask CreateInitialGameTask(Guid taskId, params DbUser[] users) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Game,
            Data = string.Join(",", users.Select(u => $"{u.Id:D}:{u.UserName}")),
            Status = QueuedTaskStatus.Created,
        };
    }
}