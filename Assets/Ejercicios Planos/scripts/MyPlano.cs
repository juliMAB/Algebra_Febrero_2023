using juli;

namespace UnityEngine
{
    public partial struct MyPlane   
    {
        // sizeof(Plane) is not const in C# and so cannot be used in fixed arrays, so we define it here
        internal const int size = 16;

        Vec3 m_Normal;
        float m_Distance;

        // Normal vector of the plane.
        public Vec3 normal
        {
            get { return m_Normal; }
            set { m_Normal = value; }
        }
        // Distance from the origin to the plane.
        public float distance
        {
            get { return m_Distance; }
            set { m_Distance = value; }
        }

        // Creates a plane.
        public MyPlane(Vec3 inNormal, Vec3 inPoint)
        {
            m_Normal = Vec3.Normalize(inNormal);
            m_Distance = -Vec3.Dot(m_Normal, inPoint);
        }

        // Creates a plane.
        public MyPlane(Vec3 inNormal, float d)
        {
            m_Normal = Vec3.Normalize(inNormal);
            m_Distance = d;
        }

        // Creates a plane.
        public MyPlane(Vec3 a, Vec3 b, Vec3 c)
        {
            m_Normal = Vec3.Normalize(Vec3.Cross(b - a, c - a));
            m_Distance = -Vec3.Dot(m_Normal, a);
        }

        // Sets a plane using a point that lies within it plus a normal to orient it (note that the normal must be a normalized vector).
        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
        {
            m_Normal = Vec3.Normalize(inNormal);
            m_Distance = -Vec3.Dot(inNormal, inPoint);
        }

        // Sets a plane using three points that lie within it.  The points go around clockwise as you look down on the top surface of the plane.
        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            m_Normal = Vec3.Normalize(Vec3.Cross(b - a, c - a));
            m_Distance = -Vec3.Dot(m_Normal, a);
        }

        // Make the plane face the opposite direction
        public void Flip() { m_Normal = -m_Normal; m_Distance = -m_Distance; }

        // Return a version of the plane that faces the opposite direction
        public MyPlane flipped { get { return new MyPlane(-m_Normal, -m_Distance); } }

        // Translates the plane into a given direction
        public void Translate(Vec3 translation) { m_Distance += Vec3.Dot(m_Normal, translation); }

        // Creates a plane that's translated into a given direction
        public static MyPlane Translate(MyPlane plane, Vec3 translation) { return new MyPlane(plane.m_Normal, plane.m_Distance += Vec3.Dot(plane.m_Normal, translation)); }

        // Calculates the closest point on the plane.
        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            var pointToPlaneDistance = Vec3.Dot(m_Normal, point) + m_Distance;
            return point - (m_Normal * pointToPlaneDistance);
        }

        // Returns a signed distance from plane to point.
        public float GetDistanceToPoint(Vec3 point) { return Vec3.Dot(m_Normal, point) + m_Distance; }

        // Is a point on the positive side of the plane?
        public bool GetSide(Vec3 point) { return Vec3.Dot(m_Normal, point) + m_Distance > 0.0F; }

        // Are two points on the same side of the plane?
        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            float d0 = GetDistanceToPoint(inPt0);
            float d1 = GetDistanceToPoint(inPt1);
            return (d0 > 0.0f && d1 > 0.0f) ||
                (d0 <= 0.0f && d1 <= 0.0f);
        }
    }
}