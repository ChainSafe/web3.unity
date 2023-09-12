using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.Field;

public class ECPointArithmetic
{
    ECCurve ec;
    IFiniteField ef;

    private BigInteger x;
    private BigInteger y;
    private BigInteger z;
    private BigInteger? zinv;
    private BigInteger one = BigInteger.One;
    private BigInteger zero = BigInteger.Zero;
    private bool infinity;

    public ECPointArithmetic(ECCurve ec, BigInteger x, BigInteger y, BigInteger z)
    {
        this.ec = ec;
        this.x = x;
        this.y = y;
        this.ef = ec.Field;

        // Projective coordinates: either zinv == null or z * zinv == 1
        // z and zinv are just BigIntegers, not fieldElements
        if (z == null)
        {
            this.z = BigInteger.One;
        }
        else
        {
            this.z = z;
        }
        this.zinv = null;
        infinity = false;
    }

    public BigInteger getX()
    {
        if (this.zinv == null)
        {
            this.zinv = this.z.ModInverse(this.ef.Characteristic);
        }
        return this.x.Multiply(this.zinv).Mod(this.ef.Characteristic);
    }

    public BigInteger getY()
    {
        if (this.zinv == null)
        {
            this.zinv = this.z.ModInverse(this.ef.Characteristic);
        }
        return this.y.Multiply(this.zinv).Mod(this.ef.Characteristic);
    }

    public bool pointEquals(ECPointArithmetic other)
    {
        if (other == this)
        {
            return true;
        }
        if (this.isInfinity())
        {
            return other.isInfinity();
        }
        if (other.isInfinity())
        {
            return this.isInfinity();
        }
        BigInteger u, v;
        // u = Y2 * Z1 - Y1 * Z2
        u = other.y.Multiply(this.z).Subtract(this.y.Multiply(other.z)).Mod(this.ef.Characteristic);
        if (!u.Equals(BigInteger.Zero))
        {
            return false;
        }
        // v = X2 * Z1 - X1 * Z2
        v = other.x.Multiply(this.z).Subtract(this.x.Multiply(other.z)).Mod(this.ef.Characteristic);
        return v.Equals(BigInteger.Zero);
    }

    public bool isInfinity()
    {

        if ((this.x == zero) && (this.y == zero))
        {
            return true;
        }
        return this.z.Equals(BigInteger.Zero) && !this.y.Equals(BigInteger.Zero);

    }

    public ECPointArithmetic negate()
    {
        return new ECPointArithmetic(this.ec, this.x, this.y.Negate(), this.z);
    }

    public ECPointArithmetic add(ECPointArithmetic b)
    {
        if (this.isInfinity())
        {
            return b;
        }
        if (b.isInfinity())
        {
            return this;
        }
        ECPointArithmetic R = new ECPointArithmetic(this.ec, zero, zero, null);
        // u = Y2 * Z1 - Y1 * Z2
        BigInteger u = b.y.Multiply(this.z).Subtract(this.y.Multiply(b.z)).Mod(this.ef.Characteristic);
        // v = X2 * Z1 - X1 * Z2
        BigInteger v = b.x.Multiply(this.z).Subtract(this.x.Multiply(b.z)).Mod(this.ef.Characteristic);

        if (BigInteger.Zero.Equals(v))
        {
            if (BigInteger.Zero.Equals(u))
            {
                return this.twice(); // this == b, so double
            }

            infinity = true; // this = -b, so infinity
            return R;
        }

        BigInteger THREE = new BigInteger("3");
        BigInteger x1 = this.x;
        BigInteger y1 = this.y;
        BigInteger x2 = b.x;
        BigInteger y2 = b.y;

        BigInteger v2 = v.Pow(2);
        BigInteger v3 = v2.Multiply(v);
        BigInteger x1v2 = x1.Multiply(v2);
        BigInteger zu2 = u.Pow(2).Multiply(this.z);

        // x3 = v * (z2 * (z1 * u^2 - 2 * x1 * v^2) - v^3)
        BigInteger x3 = zu2.Subtract(x1v2.ShiftLeft(1)).Multiply(b.z).Subtract(v3).Multiply(v).Mod(this.ef.Characteristic);

        // y3 = z2 * (3 * x1 * u * v^2 - y1 * v^3 - z1 * u^3) + u * v^3
        BigInteger y3 = x1v2.Multiply(THREE).Multiply(u).Subtract(y1.Multiply(v3)).Subtract(zu2.Multiply(u)).Multiply(b.z).Add(u.Multiply(v3)).Mod(this.ef.Characteristic);

        // z3 = v^3 * z1 * z2
        BigInteger z3 = v3.Multiply(this.z).Multiply(b.z).Mod(this.ef.Characteristic);

        return new ECPointArithmetic(this.ec, x3, y3, z3);
    }

    public ECPointArithmetic twice()
    {
        if (this.isInfinity())
        {
            return this;
        }
        ECPointArithmetic R = new ECPointArithmetic(this.ec, zero, zero, null);
        if (this.y.SignValue == 0)
        {
            infinity = true;
            return R;
        }

        BigInteger THREE = new BigInteger("3");
        BigInteger x1 = this.x;
        BigInteger y1 = this.y;

        BigInteger y1z1 = y1.Multiply(this.z);
        BigInteger y1sqz1 = y1z1.Multiply(y1).Mod(this.ef.Characteristic);
        BigInteger a = this.ec.A.ToBigInteger();

        // w = 3 * x1^2 + a * z1^2
        BigInteger w = x1.Pow(2).Multiply(THREE);

        if (!BigInteger.Zero.Equals(a))
        {
            w = w.Add(this.z.Pow(2).Multiply(a));
        }

        w = w.Mod(this.ef.Characteristic);

        // x3 = 2 * y1 * z1 * (w^2 - 8 * x1 * y1^2 * z1)
        BigInteger x3 = w.Pow(2).Subtract(x1.ShiftLeft(3).Multiply(y1sqz1)).ShiftLeft(1).Multiply(y1z1).Mod(this.ef.Characteristic);

        // y3 = 4 * y1^2 * z1 * (3 * w * x1 - 2 * y1^2 * z1) - w^3
        BigInteger y3 = (w.Multiply(THREE).Multiply(x1).Subtract(y1sqz1.ShiftLeft(1))).ShiftLeft(2).Multiply(y1sqz1).Subtract(w.Pow(2).Multiply(w)).Mod(this.ef.Characteristic);

        // z3 = 8 * (y1 * z1)^3
        BigInteger z3 = y1z1.Pow(2).Multiply(y1z1).ShiftLeft(3).Mod(this.ef.Characteristic);

        return new ECPointArithmetic(this.ec, x3, y3, z3);
    }

    public ECPointArithmetic multiply(BigInteger k)
    {
        if (this.isInfinity())
        {
            return this;
        }

        ECPointArithmetic R = new ECPointArithmetic(this.ec, zero, zero, null);
        if (k.SignValue == 0)
        {
            infinity = true;
            return R;
        }

        BigInteger e = k;
        BigInteger h = e.Multiply(new BigInteger("3"));

        ECPointArithmetic neg = this.negate();
        R = this;

        int i;
        for (i = h.BitLength - 2; i > 0; --i)
        {
            R = R.twice();
            bool hBit = h.TestBit(i);
            bool eBit = e.TestBit(i);

            if (hBit != eBit)
            {
                R = R.add(hBit ? this : neg);
            }
        }

        return R;
    }
}
