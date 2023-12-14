using System.Collections.Generic;
using Plugins.CountlySDK.Models;
using Plugins.CountlySDK.Persistance.Entities;
using Plugins.iBoxDB;
using UnityEngine;

namespace Plugins.CountlySDK.Persistance.Repositories
{
    public abstract class Repository<TEntity, TModel> where TEntity : class, IEntity, new() where TModel : IModel
    {
        private readonly Dao<TEntity> _dao;
        private readonly CountlyLogHelper Log;

        protected Repository(Dao<TEntity> dao, CountlyLogHelper log)
        {
            _dao = dao;
            this.Log = log;
        }

        internal Queue<TModel> Models { get; } = new Queue<TModel>();

        internal int Count => Models.Count;

        public virtual void Initialize()
        {
            RefreshMemoryCache();
        }

        public virtual bool Enqueue(TModel model)
        {
            Log.Verbose("[Repository] Enqueue, TModel: " + model);

            if (!ValidateModelBeforeEnqueue(model)) {
                return false;
            }

            Models.Enqueue(model);
            TEntity entity = ConvertModelToEntity(model);
            bool res = _dao.Save(entity);

            return res;
        }

        public virtual TModel Dequeue()
        {
            TModel model = Models.Dequeue();
            _dao.Remove(model.Id);

            Log.Verbose("[Repository] Dequeue, TModel: " + model.ToString());
            return model;
        }

        public virtual bool Update(TModel model)
        {
            TEntity entity = ConvertModelToEntity(model);
            return _dao.Update(entity);
        }

        public virtual void RefreshMemoryCache()
        {
            Models.Clear();

            List<TEntity> entities = _dao.LoadAll();
            foreach (TEntity entity in entities) {
                TModel model = ConvertEntityToModel(entity);
                if (!ValidateModelBeforeEnqueue(model)) {
                    continue;
                }

                // Log.Verbose("[Repository] Loaded model: " + model.ToString());

                Models.Enqueue(model);
            }
            Log.Verbose("[Repository] Loaded entities of type " + typeof(TEntity).Name + " from db:" + entities.Count);
        }

        /// <summary>
        /// Should only be used for migration or tests
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual void DeleteEntry(TModel model)
        {
            _dao.Remove(model.Id);
            Log.Verbose("[Repository] DeleteEntry, TModel: " + model.ToString());
        }


        public virtual void Clear()
        {
            Models.Clear();
            _dao.RemoveAll();
        }

        protected abstract TModel ConvertEntityToModel(TEntity entity);
        protected abstract TEntity ConvertModelToEntity(TModel model);

        protected long GenerateNewId()
        {
            return _dao.GenerateNewId();
        }

        protected abstract bool ValidateModelBeforeEnqueue(TModel model);
    }
}
