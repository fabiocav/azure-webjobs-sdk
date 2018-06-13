﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;

namespace FakeStorage
{
    internal class FakeStorageBlockBlob : CloudBlockBlob
    {
        private readonly MemoryBlobStore _store;
        private readonly string _blobName;
        private readonly FakeStorageBlobContainer _parent;
        private readonly string _containerName;
        private readonly IDictionary<string, string> _metadata;
        private readonly FakeStorageBlobProperties _properties;

        public FakeStorageBlockBlob(string blobName, FakeStorageBlobContainer parent, FakeStorageBlobProperties properties = null) : 
            base(parent.GetBlobUri(blobName))
        {
            _store = parent._store;
            _blobName = blobName;
            _parent = parent;
            _containerName = parent.Name;
            _metadata = new Dictionary<string, string>();
            if (properties != null)
            {
                _properties = properties;
            }
            else
            {
                _properties = new FakeStorageBlobProperties();
            }
        }
        
        public override Task AbortCopyAsync(string copyId)
        {
            throw new NotImplementedException();
            return base.AbortCopyAsync(copyId);
        }

        public override Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.AbortCopyAsync(copyId, accessCondition, options, operationContext);
        }

        public override Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.AbortCopyAsync(copyId, accessCondition, options, operationContext, cancellationToken);
        }

        public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            if (proposedLeaseId != null)
            {
                throw new NotImplementedException();
            }

            string leaseId = _store.AcquireLease(_containerName, _blobName, leaseTime);
            return Task.FromResult(leaseId);
        }

        public override Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId = null)
        {
            //throw new NotImplementedException();
            return base.AcquireLeaseAsync(leaseTime, proposedLeaseId);
        }

        public override Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            //throw new NotImplementedException();
            return base.AcquireLeaseAsync(leaseTime, proposedLeaseId, accessCondition, options, operationContext);
        }

        public override Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return base.AcquireLeaseAsync(leaseTime, proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod)
        {
            throw new NotImplementedException();
            return base.BreakLeaseAsync(breakPeriod);
        }

        public override Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.BreakLeaseAsync(breakPeriod, accessCondition, options, operationContext);
        }

        public override Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.BreakLeaseAsync(breakPeriod, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition)
        {
            throw new NotImplementedException();
            return base.ChangeLeaseAsync(proposedLeaseId, accessCondition);
        }

        public override Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.ChangeLeaseAsync(proposedLeaseId, accessCondition, options, operationContext);
        }

        public override Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.ChangeLeaseAsync(proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<CloudBlockBlob> CreateSnapshotAsync()
        {
            throw new NotImplementedException();
            return base.CreateSnapshotAsync();
        }

        public override Task<CloudBlockBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.CreateSnapshotAsync(metadata, accessCondition, options, operationContext);
        }

        public override Task<CloudBlockBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.CreateSnapshotAsync(metadata, accessCondition, options, operationContext, cancellationToken);
        }

        public Task DeleteAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            throw new NotImplementedException();
        }

        public override Task DeleteAsync()
        {
            throw new NotImplementedException();
            return base.DeleteAsync();
        }

        public override Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DeleteAsync(deleteSnapshotsOption, accessCondition, options, operationContext);
        }

        public override Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DeleteAsync(deleteSnapshotsOption, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<bool> DeleteIfExistsAsync()
        {
            throw new NotImplementedException();
            return base.DeleteIfExistsAsync();
        }

        public override Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DeleteIfExistsAsync(deleteSnapshotsOption, accessCondition, options, operationContext);
        }

        public override Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DeleteIfExistsAsync(deleteSnapshotsOption, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync()
        {
            throw new NotImplementedException();
            return base.DownloadBlockListAsync();
        }

        public override Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync(BlockListingFilter blockListingFilter, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadBlockListAsync(blockListingFilter, accessCondition, options, operationContext);
        }

        public override Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync(BlockListingFilter blockListingFilter, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadBlockListAsync(blockListingFilter, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToByteArrayAsync(target, index, blobOffset, length);
        }

        public override Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToByteArrayAsync(target, index, blobOffset, length, accessCondition, options, operationContext);
        }

        public override Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToByteArrayAsync(target, index, blobOffset, length, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToStreamAsync(target, offset, length);
        }

        public override Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToStreamAsync(target, offset, length, accessCondition, options, operationContext);
        }

        public override Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadRangeToStreamAsync(target, offset, length, accessCondition, options, operationContext, cancellationToken);
        }

        public override async Task<string> DownloadTextAsync()
        {
            using (Stream stream = await OpenReadAsync(CancellationToken.None))
            {
                using (TextReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public override Task<string> DownloadTextAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadTextAsync(accessCondition, options, operationContext);
        }

        public override Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadTextAsync(encoding, accessCondition, options, operationContext);
        }

        public override Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadTextAsync(encoding, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<int> DownloadToByteArrayAsync(byte[] target, int index)
        {
            throw new NotImplementedException();
            return base.DownloadToByteArrayAsync(target, index);
        }

        public override Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadToByteArrayAsync(target, index, accessCondition, options, operationContext);
        }

        public override Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadToByteArrayAsync(target, index, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task DownloadToFileAsync(string path, FileMode mode)
        {
            throw new NotImplementedException();
            return base.DownloadToFileAsync(path, mode);
        }

        public override Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadToFileAsync(path, mode, accessCondition, options, operationContext);
        }

        public override Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadToFileAsync(path, mode, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task DownloadToStreamAsync(Stream target)
        {
            throw new NotImplementedException();
            return base.DownloadToStreamAsync(target);
        }

        public override Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.DownloadToStreamAsync(target, accessCondition, options, operationContext);
        }

        public override Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.DownloadToStreamAsync(target, accessCondition, options, operationContext, cancellationToken);
        }

        public override bool Equals(object obj)
        {
            if (obj is FakeStorageBlockBlob other) {
                return other.Uri == this.Uri;
            }
            return false;
        }

        public Task<bool> ExistsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.Exists(_containerName, _blobName));
        }

        public override Task<bool> ExistsAsync()
        {
            return Task.FromResult(_store.Exists(_containerName, _blobName));            
        }

        public override Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext)
        {
            return Task.FromResult(_store.Exists(_containerName, _blobName));
        }

        public override Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.Exists(_containerName, _blobName));
        }

        public override Task FetchAttributesAsync()
        {
            return base.FetchAttributesAsync();
        }

        public override Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.FetchAttributesAsync(accessCondition, options, operationContext);
        }

        public override Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            BlobAttributes attributes = _store.FetchAttributes(_containerName, _blobName);
            _properties.ETag = attributes.ETag;
            _properties.LastModified = attributes.LastModified;
            _metadata.Clear();

            foreach (KeyValuePair<string, string> item in attributes.Metadata)
            {
                _metadata.Add(item);
            }

            return Task.FromResult(0);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.OpenRead(_containerName, _blobName));
        }

        public override Task<Stream> OpenReadAsync()
        {
            return base.OpenReadAsync();
        }

        public override Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.OpenReadAsync(accessCondition, options, operationContext);
        }

        public override Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.OpenRead(_containerName, _blobName));
        }

        public Task<CloudBlobStream> OpenWriteAsync(CancellationToken cancellationToken)
        {
            CloudBlobStream stream = _store.OpenWriteBlock(_containerName, _blobName, _metadata);
            return Task.FromResult(stream);
        }

        public override Task<CloudBlobStream> OpenWriteAsync()
        {
            return base.OpenWriteAsync();
        }

        public override Task<CloudBlobStream> OpenWriteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.OpenWriteAsync(accessCondition, options, operationContext);
        }

        public override Task<CloudBlobStream> OpenWriteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            CloudBlobStream stream = _store.OpenWriteBlock(_containerName, _blobName, _metadata);
            return Task.FromResult(stream);
        }

        public override Task PutBlockAsync(string blockId, Stream blockData, string contentMD5)
        {
            throw new NotImplementedException();
            return base.PutBlockAsync(blockId, blockData, contentMD5);
        }

        public override Task PutBlockAsync(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.PutBlockAsync(blockId, blockData, contentMD5, accessCondition, options, operationContext);
        }

        public override Task PutBlockAsync(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.PutBlockAsync(blockId, blockData, contentMD5, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task PutBlockListAsync(IEnumerable<string> blockList)
        {
            throw new NotImplementedException();
            return base.PutBlockListAsync(blockList);
        }

        public override Task PutBlockListAsync(IEnumerable<string> blockList, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            throw new NotImplementedException();
            return base.PutBlockListAsync(blockList, accessCondition, options, operationContext);
        }

        public override Task PutBlockListAsync(IEnumerable<string> blockList, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            return base.PutBlockListAsync(blockList, accessCondition, options, operationContext, cancellationToken);
        }

         public override Task ReleaseLeaseAsync(AccessCondition accessCondition)
        {
            return base.ReleaseLeaseAsync(accessCondition);
        }

        public override Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.ReleaseLeaseAsync(accessCondition, options, operationContext);
        }

        public override Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            if (accessCondition == null)
            {
                throw new ArgumentNullException("accessCondition");
            }

            if (options != null)
            {
                throw new NotImplementedException();
            }

            if (operationContext != null)
            {
                throw new NotImplementedException();
            }

            if (accessCondition.IfMatchETag != null ||
                accessCondition.IfModifiedSinceTime.HasValue ||
                accessCondition.IfNoneMatchETag != null ||
                accessCondition.IfNotModifiedSinceTime.HasValue ||
                accessCondition.IfSequenceNumberEqual.HasValue ||
                accessCondition.IfSequenceNumberLessThan.HasValue ||
                accessCondition.IfSequenceNumberLessThanOrEqual.HasValue)
            {
                throw new NotImplementedException();
            }

            _store.ReleaseLease(_containerName, _blobName, accessCondition.LeaseId);
            return Task.FromResult(0);
        }

        public override Task RenewLeaseAsync(AccessCondition accessCondition)
        {
            return base.RenewLeaseAsync(accessCondition);
        }

        public override Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.RenewLeaseAsync(accessCondition, options, operationContext);
        }

        public override Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.RenewLeaseAsync(accessCondition, options, operationContext, cancellationToken);
        }

        public override Task SetMetadataAsync()
        {
            return base.SetMetadataAsync();
        }

        public override Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.SetMetadataAsync(accessCondition, options, operationContext);
        }

        public override Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            if (options != null)
            {
                throw new NotImplementedException();
            }

            if (operationContext != null)
            {
                throw new NotImplementedException();
            }

            if (accessCondition != null &&
                (accessCondition.IfMatchETag != null ||
                accessCondition.IfModifiedSinceTime.HasValue ||
                accessCondition.IfNoneMatchETag != null ||
                accessCondition.IfNotModifiedSinceTime.HasValue ||
                accessCondition.IfSequenceNumberEqual.HasValue ||
                accessCondition.IfSequenceNumberLessThan.HasValue ||
                accessCondition.IfSequenceNumberLessThanOrEqual.HasValue))
            {
                throw new NotImplementedException();
            }

            string leaseId;

            if (accessCondition != null)
            {
                leaseId = accessCondition.LeaseId;
            }
            else
            {
                leaseId = null;
            }

            _store.SetMetadata(_containerName, _blobName, _metadata, leaseId);
            return Task.FromResult(0);
        }

        public override Task SetPropertiesAsync()
        {
            return base.SetPropertiesAsync();
        }

        public override Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.SetPropertiesAsync(accessCondition, options, operationContext);
        }

        public override Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.SetPropertiesAsync(accessCondition, options, operationContext, cancellationToken);
        }

        public override Task SetStandardBlobTierAsync(StandardBlobTier standardBlobTier)
        {
            return base.SetStandardBlobTierAsync(standardBlobTier);
        }

        public override Task SetStandardBlobTierAsync(StandardBlobTier standardBlobTier, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.SetStandardBlobTierAsync(standardBlobTier, accessCondition, options, operationContext);
        }

        public override Task SetStandardBlobTierAsync(StandardBlobTier standardBlobTier, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.SetStandardBlobTierAsync(standardBlobTier, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<CloudBlob> SnapshotAsync()
        {
            return base.SnapshotAsync();
        }

        public override Task<CloudBlob> SnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.SnapshotAsync(metadata, accessCondition, options, operationContext);
        }

        public override Task<CloudBlob> SnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.SnapshotAsync(metadata, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task<string> StartCopyAsync(Uri source)
        {
            return base.StartCopyAsync(source);
        }

        public override Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext);
        }

        public override Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
        }

        public override Task<string> StartCopyAsync(CloudBlockBlob source)
        {
            return base.StartCopyAsync(source);
        }

        public override Task<string> StartCopyAsync(CloudFile source)
        {
            return base.StartCopyAsync(source);
        }

        public override Task<string> StartCopyAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext);
        }

        public override Task<string> StartCopyAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
        }

        public override Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext);
        }

        public override Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
        {
            return base.UploadFromByteArrayAsync(buffer, index, count);
        }

        public override Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext);
        }

        public override Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task UploadFromFileAsync(string path)
        {
            return base.UploadFromFileAsync(path);
        }

        public override Task UploadFromFileAsync(string path, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadFromFileAsync(path, accessCondition, options, operationContext);
        }

        public override Task UploadFromFileAsync(string path, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.UploadFromFileAsync(path, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task UploadFromStreamAsync(Stream source)
        {
            return base.UploadFromStreamAsync(source);
        }

        public override Task UploadFromStreamAsync(Stream source, long length)
        {
            return base.UploadFromStreamAsync(source, length);
        }

        public override Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadFromStreamAsync(source, accessCondition, options, operationContext);
        }

        public override Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadFromStreamAsync(source, length, accessCondition, options, operationContext);
        }

        public override Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.UploadFromStreamAsync(source, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            return base.UploadFromStreamAsync(source, length, accessCondition, options, operationContext, cancellationToken);
        }

        public override Task UploadTextAsync(string content)
        {
            return base.UploadTextAsync(content);
        }

        public override Task UploadTextAsync(string content, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadTextAsync(content, accessCondition, options, operationContext);
        }

        public override Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
        {
            return base.UploadTextAsync(content, encoding, accessCondition, options, operationContext);
        }

        public override Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
        {
            using (CloudBlobStream stream = _store.OpenWriteBlock(_containerName, _blobName, _metadata))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                stream.Write(buffer, 0, buffer.Length);
                stream.CommitAsync().Wait();
            }

            return Task.FromResult(0);
        }
    }
}
