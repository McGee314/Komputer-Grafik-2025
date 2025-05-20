// Abstraction
class Window {
public:
    Window(View* contents);

    // Permintaan dari aplikasi
    virtual void DrawContents();
    virtual void Open();
    virtual void Close();
    virtual void Iconify();
    virtual void Deiconify();

    // Didelegasikan ke implementasi
    virtual void SetOrigin(const Point& at);
    virtual void SetExtent(const Point& extent);
    virtual void Raise();
    virtual void Lower();
    virtual void DrawLine(const Point&, const Point&);
    virtual void DrawRect(const Point&, const Point&);
    virtual void DrawPolygon(const Point[], int n);
    virtual void DrawText(const char*, const Point&);

protected:
    WindowImp* GetWindowImp();
    View* GetView();

private:
    WindowImp* _imp;
    View* _contents;
};

// Implementor
class WindowImp {
public:
    virtual void ImpTop() = 0;
    virtual void ImpBottom() = 0;
    virtual void ImpSetExtent(const Point&) = 0;
    virtual void ImpSetOrigin(const Point&) = 0;
    virtual void DeviceRect(Coord, Coord, Coord, Coord) = 0;
    virtual void DeviceText(const char*, Coord, Coord) = 0;
    virtual void DeviceBitmap(const char*, Coord, Coord) = 0;
protected:
    WindowImp();
};

// RefinedAbstraction
class IconWindow : public Window {
public:
    virtual void DrawContents();
private:
    const char* _bitmapName;
};

void IconWindow::DrawContents() {
    WindowImp* imp = GetWindowImp();
    if (imp != nullptr) {
        imp->DeviceBitmap(_bitmapName, 0.0, 0.0);
    }
}

// ConcreteImplementor A
class XWindowImp : public WindowImp {
public:
    virtual void DeviceRect(Coord x0, Coord y0, Coord x1, Coord y1) override {
        int x = round(min(x0, x1));
        int y = round(min(y0, y1));
        int w = round(abs(x0 - x1));
        int h = round(abs(y0 - y1));
        XDrawRectangle(_dpy, _winid, _gc, x, y, w, h);
    }
private:
    Display* _dpy;
    Drawable _winid;
    GC _gc;
};

// ConcreteImplementor B
class PMWindowImp : public WindowImp {
public:
    virtual void DeviceRect(Coord x0, Coord y0, Coord x1, Coord y1) override {
        Coord left = min(x0, x1);
        Coord right = max(x0, x1);
        Coord bottom = min(y0, y1);
        Coord top = max(y0, y1);
        PPOINTL point[4] = {
            {left, top}, {right, top}, {right, bottom}, {left, bottom}
        };
        if ((GpiBeginPath(_hps, 1L) == false) ||
            (GpiSetCurrentPosition(_hps, &point[3]) == false) ||
            (GpiPolyLine(_hps, 4L, point) == GPI_ERROR) ||
            (GpiEndPath(_hps) == false)) {
            // handle error
        } else {
            GpiStrokePath(_hps, 1L, 0L);
        }
    }
private:
    HPS _hps;
};

// Factory method (digunakan oleh Window untuk mengambil WindowImp yang sesuai)
WindowImp* Window::GetWindowImp() {
    if (_imp == nullptr) {
        _imp = WindowSystemFactory::Instance()->MakeWindowImp();
    }
    return _imp;
}