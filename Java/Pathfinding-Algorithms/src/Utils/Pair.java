package Utils;

public class Pair <T, V> {

    public T value1;
    public V value2;
    public Pair(T a, V b)
    {
        value1 = a;
        value2 = b;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null || getClass() != obj.getClass()) {
            return false;
        }
        Pair<?, ?> other = (Pair<?, ?>) obj;
        return value1 == other.value1 &&
                value2 == other.value2;
    }

    public static boolean equals(Pair<?, ?> a, Pair<?, ?> b) {
        if (a == null && b == null) {
            return true;
        }
        if (a == null || b == null) {
            return false;
        }
        return a.value1 == b.value1 &&
                a.value2 == b.value2;
    }
}
