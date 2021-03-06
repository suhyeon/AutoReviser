﻿namespace AutoReviser
{
    using System;
    using System.Collections.Immutable;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImmutableExtensions_specs
    {
        public class SimpleImmutableObject
        {
            public SimpleImmutableObject(Guid alfa, int bravo, string charlie)
                => (Alfa, Bravo, Charlie) = (alfa, bravo, charlie);

            public Guid Alfa { get; }

            public int Bravo { get; }

            public string Charlie { get; }
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_new_property_value(
            SimpleImmutableObject seed, Guid alfa)
        {
            // Act
            SimpleImmutableObject actual = seed.Revise(x => x.Alfa == alfa);

            // Assert
            actual.Alfa.Should().Be(alfa);
            actual.Bravo.Should().Be(seed.Bravo);
            actual.Charlie.Should().Be(seed.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_two_new_property_values(
            SimpleImmutableObject seed, Guid alfa, int bravo)
        {
            // Act
            SimpleImmutableObject actual =
                seed.Revise(x => x.Alfa == alfa && x.Bravo == bravo);

            // Assert
            actual.Alfa.Should().Be(alfa);
            actual.Bravo.Should().Be(bravo);
            actual.Charlie.Should().Be(seed.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_three_new_property_values(
            SimpleImmutableObject seed, Guid alfa, int bravo, string charlie)
        {
            // Act
            SimpleImmutableObject actual = seed.Revise(
                x =>
                x.Alfa == alfa &&
                x.Bravo == bravo &&
                x.Charlie == charlie);

            // Assert
            actual.Alfa.Should().Be(alfa);
            actual.Bravo.Should().Be(bravo);
            actual.Charlie.Should().Be(charlie);
        }

        public class ComplexImmutableObject
        {
            public ComplexImmutableObject(
                Guid delta, SimpleImmutableObject echo)
            {
                (Delta, Echo) = (delta, echo);
            }

            public Guid Delta { get; }

            public SimpleImmutableObject Echo { get; }
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_new_deep_property_value(
            ComplexImmutableObject seed, Guid alfa)
        {
            // Act
            ComplexImmutableObject actual =
                seed.Revise(x => x.Echo.Alfa == alfa);

            // Assert
            actual.Delta.Should().Be(seed.Delta);
            actual.Echo.Alfa.Should().Be(alfa);
            actual.Echo.Bravo.Should().Be(seed.Echo.Bravo);
            actual.Echo.Charlie.Should().Be(seed.Echo.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_two_new_deep_property_values(
            ComplexImmutableObject seed, Guid alfa, int bravo)
        {
            // Act
            ComplexImmutableObject actual = seed.Revise(
                x =>
                x.Echo.Alfa == alfa &&
                x.Echo.Bravo == bravo);

            // Assert
            actual.Delta.Should().Be(seed.Delta);
            actual.Echo.Alfa.Should().Be(alfa);
            actual.Echo.Bravo.Should().Be(bravo);
            actual.Echo.Charlie.Should().Be(seed.Echo.Charlie);
        }

        public class MoreComplexImmutableObject
        {
            public MoreComplexImmutableObject(
                Guid foxtrot, ComplexImmutableObject golf)
            {
                (Foxtrot, Golf) = (foxtrot, golf);
            }

            public Guid Foxtrot { get; }

            public ComplexImmutableObject Golf { get; }
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_new_more_deep_property_value(
            MoreComplexImmutableObject seed, Guid alfa)
        {
            // Act
            MoreComplexImmutableObject actual =
                seed.Revise(x => x.Golf.Echo.Alfa == alfa);

            // Assert
            actual.Foxtrot.Should().Be(seed.Foxtrot);
            actual.Golf.Delta.Should().Be(seed.Golf.Delta);
            actual.Golf.Echo.Alfa.Should().Be(alfa);
            actual.Golf.Echo.Bravo.Should().Be(seed.Golf.Echo.Bravo);
            actual.Golf.Echo.Charlie.Should().Be(seed.Golf.Echo.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_two_new_more_deep_property_values(
            MoreComplexImmutableObject seed, Guid alfa, int bravo)
        {
            // Act
            MoreComplexImmutableObject actual = seed.Revise(
                x =>
                x.Golf.Echo.Alfa == alfa &&
                x.Golf.Echo.Bravo == bravo);

            // Assert
            actual.Foxtrot.Should().Be(seed.Foxtrot);
            actual.Golf.Delta.Should().Be(seed.Golf.Delta);
            actual.Golf.Echo.Alfa.Should().Be(alfa);
            actual.Golf.Echo.Bravo.Should().Be(bravo);
            actual.Golf.Echo.Charlie.Should().Be(seed.Golf.Echo.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_with_complex_condition(
            MoreComplexImmutableObject seed,
            Guid alfa,
            Guid delta,
            Guid foxtrot)
        {
            // Act
            MoreComplexImmutableObject actual = seed.Revise(
                x =>
                x.Golf.Echo.Alfa == alfa &&
                x.Golf.Delta == delta &&
                x.Foxtrot == foxtrot);

            // Assert
            actual.Foxtrot.Should().Be(foxtrot);
            actual.Golf.Delta.Should().Be(delta);
            actual.Golf.Echo.Alfa.Should().Be(alfa);
            actual.Golf.Echo.Bravo.Should().Be(seed.Golf.Echo.Bravo);
            actual.Golf.Echo.Charlie.Should().Be(seed.Golf.Echo.Charlie);
        }

        [TestMethod]
        [AutoData]
        public void Revise_updates_element_in_immutable_array(
            [ImmutableArrayCustomization] ImmutableArray<string> seed,
            string element)
        {
            ImmutableArray<string> actual = seed.Revise(x => x[1] == element);
            actual[1].Should().Be(element);
        }

        [TestMethod, AutoData]
        public void Revise_updates_multiple_elements_in_immutable_array(
            [ImmutableArrayCustomization] ImmutableArray<string> seed,
            string element)
        {
            ImmutableArray<string> revision = seed.Revise(
                x =>
                x[0] == element &&
                x[1] == element);

            revision[0].Should().Be(element);
            revision[1].Should().Be(element);
        }

        public class HasPrivateConstructor
        {
            private HasPrivateConstructor(int alfa, string bravo)
                => (Alfa, Bravo) = (alfa, bravo);

            public int Alfa { get; }

            public string Bravo { get; }

            public static HasPrivateConstructor Create()
                => new HasPrivateConstructor(alfa: 1, bravo: "foo");
        }

        [TestMethod]
        [AutoData]
        public void Revise_creates_new_object_using_non_public_constructor_correctly(
            int alfa, string bravo)
        {
            var seed = HasPrivateConstructor.Create();

            HasPrivateConstructor actual =
                seed.Revise(x => x.Alfa == alfa && x.Bravo == bravo);

            actual.Should().NotBeNull();
            actual.Alfa.Should().Be(alfa);
            actual.Bravo.Should().Be(bravo);
        }

        public enum Color
        {
            Red,
            Yellow,
            Blue,
        }

        public class HasEnumProperty
        {
            public HasEnumProperty(int alfa, Color bravo)
            {
                Alfa = alfa;
                Bravo = bravo;
            }

            public int Alfa { get; }

            public Color Bravo { get; }
        }

        [TestMethod, AutoData]
        public void Revise_updates_property_of_enum_type_correctly(
            HasEnumProperty seed, Color bravo)
        {
            HasEnumProperty actual = default;

            Action action = () => actual = seed.Revise(x => x.Bravo == bravo);

            action.Should().NotThrow();
            actual.Bravo.Should().Be(bravo);
        }

        public class HasImmutableArrayProperty
        {
            public HasImmutableArrayProperty(
                int alfa,
                ImmutableArray<string> bravo,
                ImmutableArray<SimpleImmutableObject> charlie,
                ImmutableArray<ComplexImmutableObject> delta)
            {
                Alfa = alfa;
                Bravo = bravo;
                Charlie = charlie;
                Delta = delta;
            }

            public int Alfa { get; }

            public ImmutableArray<string> Bravo { get; }

            public ImmutableArray<SimpleImmutableObject> Charlie { get; }

            public ImmutableArray<ComplexImmutableObject> Delta { get; }
        }

        [TestMethod, AutoData]
        public void Revise_updates_element_in_nested_immutable_array_correctly(
            [ImmutableArrayCustomization] HasImmutableArrayProperty seed,
            string element)
        {
            HasImmutableArrayProperty revision =
                seed.Revise(x => x.Bravo[1] == element);

            revision.Bravo[1].Should().Be(element);
        }

        [TestMethod, AutoData]
        public void Revise_updates_property_of_element_in_nested_immutable_array_type_correctly(
            [ImmutableArrayCustomization] HasImmutableArrayProperty seed,
            Guid element)
        {
            HasImmutableArrayProperty revision =
                seed.Revise(x => x.Charlie[1].Alfa == element);

            revision.Charlie[1].Alfa.Should().Be(element);
        }

        [TestMethod, AutoData]
        public void Revise_updates_deep_property_of_element_in_nested_immutable_array_type_correctly(
            [ImmutableArrayCustomization] HasImmutableArrayProperty seed,
            Guid element)
        {
            HasImmutableArrayProperty revision =
                seed.Revise(x => x.Delta[1].Echo.Alfa == element);

            revision.Delta[1].Echo.Alfa.Should().Be(element);
        }
    }
}
