using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Double3
    {
        #region Constructors

        /* needed if this is a class */
        public Double3()
        {
        }

        public Double3(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Double3(double _x, double _y, double _z)
        {
            X = _x;
            Y = _y;
            Z = _z;
        }


        public Double3(System.Windows.Media.Color color)
        {
            X = color.R / 255.0;
            Y = color.G / 255.0;
            Z = color.B / 255.0;
        }

        public Double3(System.Windows.Media.Media3D.Vector3D vector3D)
        {
            X = vector3D.X;
            Y = vector3D.Y;
            Z = vector3D.Z;
        }

        public Double3(System.Windows.Media.Media3D.Point3D point3D)
        {
            X = point3D.X;
            Y = point3D.Y;
            Z = point3D.Z;
        }

        #endregion Constructors

        #region Static Properties

        private static Double3 _Zero = new Double3();
        public static Double3 Zero
        {
            get { return _Zero; }
        }

        private static Double3 _One = new Double3(1.0);
        public static Double3 One
        {
            get { return _One; }
        }

        private static Double3 _XAxis = new Double3(1.0, 0.0, 0.0);
        public static Double3 XAxis
        {
            get { return _XAxis; }
        }

        private static Double3 _YAxis = new Double3(0.0, 1.0, 0.0);
        public static Double3 YAxis
        {
            get { return _YAxis; }
        }

        private static Double3 _ZAxis = new Double3(0.0, 0.0, 1.0);
        public static Double3 ZAxis
        {
            get { return _ZAxis; }
        }

        private static Double3 _PositiveInfinity = new Double3(double.PositiveInfinity);
        public static Double3 PositiveInfinity
        {
            get { return _PositiveInfinity; }
        }

        private static Double3 _NegativeInfinity = new Double3(double.NegativeInfinity);
        public static Double3 NegativeInfinity
        {
            get { return _NegativeInfinity; }
        }

        #endregion Static Properties

        #region Properties

        public double X;
        public double Y;
        public double Z;

        #endregion Properties

        #region Overloaded Operators

        public static Double3 operator /(double lhs, Double3 rhs)
        {
            return new Double3(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);
        }

        public static Double3 operator /(Double3 lhs, double rhs)
        {
            return new Double3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
        }

        public static Double3 operator +(Double3 lhs, Double3 rhs)
        {
            return new Double3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Double3 operator +(Double3 lhs, double rhs)
        {
            return new Double3(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs);
        }

        public static Double3 operator -(Double3 lhs, Double3 rhs)
        {
            return new Double3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static Double3 operator -(Double3 lhs, double rhs)
        {
            return new Double3(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs);
        }

        public static Double3 operator -(Double3 rhs)
        {
            return new Double3(- rhs.X, - rhs.Y, - rhs.Z);
        }

        public static Double3 operator *(Double3 lhs, double rhs)
        {
            return new Double3(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
        }

        public static Double3 operator *(Double3 lhs, Double3 rhs)
        {
            return new Double3(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Double3)
                return Equals((Double3)obj);
            else
                return false;
        }

        public override int GetHashCode()
            => this.X.GetHashCode() ^ (252452451 + this.Y.GetHashCode()) ^ (7*this.Z.GetHashCode());

        public bool Equals(Double3 d3)
        {
            return X == d3.X && Y == d3.Y && Z == d3.Z;
        }

        #endregion Overloaded Operators

        #region Vector Functions

        public void Add(double value)
        {
            X += value;
            Y += value;
            Z += value;
        }

        public void Add(Double3 value)
        {
            X += value.X;
            Y += value.Y;
            Z += value.Z;
        }

        public void Divide(int value)
        {
            X /= value;
            Y /= value;
            Z /= value;
        }

        internal void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Calls Normalize(double length)
        /// </summary>
        public void Normalize()
        {
            double length = this.Length();
            Normalize(length);
        }

        /// <summary>
        /// Use this normalize if you already know the length
        /// </summary>
        public void Normalize(double length)
        {
            X = X / length;
            Y = Y / length;
            Z = Z / length;
        }

        public void Clamp(double min, double max)
        {
            X = X < min ? min : X;
            Y = Y < min ? min : Y;
            Z = Z < min ? min : Z;
            
            X = X > max ? max : X;
            Y = Y > max ? max : Y;
            Z = Z > max ? max : Z;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Double3 Cross(Double3 rhs)
        {
            return new Double3(
                (Y * rhs.Z) - (Z * rhs.Y),
                (Z * rhs.X) - (X * rhs.Z),
                (X * rhs.Y) - (Y * rhs.X));
        }

        public double Dot(Double3 value)
        {
            return (X * value.X + Y * value.Y + Z * value.Z);
        }

        #endregion Vector Functions

        #region Helper Functions

        public double MaxValue()
        {
            double max = X > Y ? X : Y;
            return Z > max ? Z : max;
        }

        public double MinValue()
        {
            double min = X < Y ? X : Y;
            return Z < min ? Z : min;
        }

        public static Double3 Minimum(Double3 _Corner0, Double3 _Corner1)
        {
            return new Double3(
                _Corner0.X < _Corner1.X ? _Corner0.X : _Corner1.X,
                _Corner0.Y < _Corner1.Y ? _Corner0.Y : _Corner1.Y,
                _Corner0.Z < _Corner1.Z ? _Corner0.Z : _Corner1.Z);
        }

        public static Double3 Maximum(Double3 _Corner0, Double3 _Corner1)
        {
            return new Double3(
                _Corner0.X > _Corner1.X ? _Corner0.X : _Corner1.X,
                _Corner0.Y > _Corner1.Y ? _Corner0.Y : _Corner1.Y,
                _Corner0.Z > _Corner1.Z ? _Corner0.Z : _Corner1.Z);
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public static Double3 Parse(string p)
        {
            p = p.Replace(" ", "");
            char[] splitter = { ',' };
            string[] values = p.Split(splitter);
            return new Double3(
                double.Parse(values[0]),
                double.Parse(values[1]),
                double.Parse(values[2]));
        }

        public static Double3 CreateFromInt32(Int32 value)
        {
            return new Double3(
                ((value >> 16) & 0x000000ff) / 255.0,
                ((value >> 8) & 0x000000ff) / 255.0,
                ((value >> 0) & 0x000000ff) / 255.0);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            return System.Windows.Media.Color.FromRgb((byte)(X * 255), (byte)(Y * 255), (byte)(Z * 255));
        }

        #endregion Helper Functions
    }
}
