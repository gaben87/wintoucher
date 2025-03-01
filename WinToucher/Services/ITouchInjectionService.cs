using System.Windows;

namespace WinToucher.Services
{
    public interface ITouchInjectionService
    {
        void Initialize(uint maxTouches);
        void InjectTouchDown(int id, Point position);
        void InjectTouchMove(int id, Point position);
        void InjectTouchUp(int id, Point position);
    }
}