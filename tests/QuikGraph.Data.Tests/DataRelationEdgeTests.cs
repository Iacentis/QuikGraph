using System;
using System.Data;
using NUnit.Framework;

namespace QuikGraph.Data.Tests
{
    /// <summary>
    /// Tests for <see cref="DataRelationEdge"/>.
    ///</summary>
    [TestFixture]
    internal sealed class DataRelationEdgeTests
    {
        [Test]
        public void Construction()
        {
            DataRelation relation = SetupTestRelation();

            CheckRelation(new DataRelationEdge(relation), relation);

            #region Local functions

            DataRelation SetupTestRelation()
            {
                var dataSet = new DataSet();

                var customers = new DataTable("Customers");
                var customerIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
                customers.Columns.Add(customerIdCol);
                dataSet.Tables.Add(customers);

                var orders = new DataTable("Orders");
                var orderIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
                orders.Columns.Add(orderIdCol);
                dataSet.Tables.Add(orders);

                return new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            }

            void CheckRelation(DataRelationEdge e, DataRelation r)
            {
                Assert.That(e.Source,Is.Not.Null);
                Assert.That(r.ParentTable,Is.SameAs(e.Source));

                Assert.That(e.Target,Is.Not.Null);
                Assert.That(r.ChildTable,Is.SameAs(e.Target));
            }

            #endregion
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DataRelationEdge(null));
        }

        [Test]
        public void Equals()
        {
            CreateDataColumns(
                out DataColumn customerIdCol,
                out DataColumn orderIdCol);

            var relation1 = new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            var relation2 = new DataRelation("CustomersOrders", customerIdCol, orderIdCol);
            var relation3 = new DataRelation("CustomersOrders", orderIdCol, customerIdCol);

            Assert.That(relation1,Is.EqualTo(relation1));

            Assert.That(relation1,Is.Not.EqualTo(relation2));
            Assert.That(relation2,Is.Not.EqualTo(relation1));
            Assert.That(relation1.Equals(relation2),Is.False);
            Assert.That(relation2.Equals(relation1),Is.False);

            Assert.That(relation1,Is.Not.EqualTo(relation3));
            Assert.That(relation3,Is.Not.EqualTo(relation1));
            Assert.That(relation1.Equals(relation3),Is.False);
            Assert.That(relation3.Equals(relation1),Is.False);

            Assert.That(relation1,Is.Not.Null);
            Assert.That(relation1.Equals(null),Is.False);

            #region Local function

            void CreateDataColumns(out DataColumn custIdCol, out DataColumn ordIdCol)
            {
                var dataSet = new DataSet();

                var customers = new DataTable("Customers");
                custIdCol = new DataColumn("CustomerID", typeof(int)) { Unique = true };
                customers.Columns.Add(custIdCol);
                dataSet.Tables.Add(customers);

                var orders = new DataTable("Orders");
                ordIdCol = new DataColumn("OrderID", typeof(int)) { Unique = true };
                orders.Columns.Add(ordIdCol);
                dataSet.Tables.Add(orders);
            }

            #endregion
        }
    }
}