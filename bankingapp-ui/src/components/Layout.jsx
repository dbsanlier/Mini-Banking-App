import { NavLink, Outlet } from 'react-router-dom';
import './Layout.css';

const navItems = [
  { to: '/', label: 'Ana Sayfa', end: true },
  { to: '/musteriler', label: 'Müşteriler' },
  { to: '/hesaplar', label: 'Hesaplar' },
  { to: '/yatirim', label: 'Yatırım' },
];

function Layout() {
  return (
    <div className="layout">
      <aside className="layout__sidebar">
        <div className="layout__brand">
          <span className="layout__brand-mark">MB</span>
          <div>
            <div className="layout__brand-name">Mini Banka</div>
            <div className="layout__brand-sub">Kurumsal Panel</div>
          </div>
        </div>

        <nav className="layout__nav">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end={item.end}
              className={({ isActive }) =>
                'layout__nav-link' + (isActive ? ' layout__nav-link--active' : '')
              }
            >
              {item.label}
            </NavLink>
          ))}
        </nav>
      </aside>

      <main className="layout__main">
        <Outlet />
      </main>
    </div>
  );
}

export default Layout;