﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bedrock.Common;

namespace SmartSync.Common
{
    public class DirectoryDiff : Diff
    {
        public Storage LeftStorage
        {
            get
            {
                return leftStorage;
            }
        }
        public Directory Left
        {
            get
            {
                return leftDirectory;
            }
        }
        Entry Diff.Left
        {
            get
            {
                return leftDirectory;
            }
        }

        public Storage RightStorage
        {
            get
            {
                return rightStorage;
            }
        }
        public Directory Right
        {
            get
            {
                return rightDirectory;
            }
        }
        Entry Diff.Right
        {
            get
            {
                return rightDirectory;
            }
        }

        private Storage leftStorage, rightStorage;
        private Directory leftDirectory, rightDirectory;

        public DirectoryDiff(Storage leftStorage, Directory leftDirectory, Storage rightStorage, Directory rightDirectory)
        {
            this.leftStorage = leftStorage;
            this.rightStorage = rightStorage;
            this.leftDirectory = leftDirectory;
            this.rightDirectory = rightDirectory;
        }

        public Action GetAction(SyncType syncType)
        {
            switch (syncType)
            {
                case SyncType.Backup:
                {
                    if (Right == null)
                        return new CreateDirectoryAction(RightStorage, Left.Parent.Path, Left.Name);

                    break;
                }

                case SyncType.Clone:
                {
                    if (Left == null)
                        return new DeleteDirectoryAction(Right);
                    else if (Right == null)
                        return new CreateDirectoryAction(RightStorage, Left.Parent.Path, Left.Name);

                    break;
                }

                case SyncType.Sync:
                {
                    if (Left == null)
                        return new CreateDirectoryAction(LeftStorage, Right.Parent.Path, Right.Name);
                    else if (Right == null)
                        return new CreateDirectoryAction(RightStorage, Left.Parent.Path, Left.Name);

                    break;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return (leftDirectory?.Path ?? "null") + " - " + (rightDirectory?.Path ?? "null");
        }
    }
}