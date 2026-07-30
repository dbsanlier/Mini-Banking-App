import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams, Link } from 'react-router-dom';
import { createHesap } from '../services/hesapService';
import { getMusteriler } from '../services/musteriService';
import './HesapForm.css';

function HesapForm() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const preselectedMusteriId = searchParams.get('musteriId') || '';

  const [musteriler, setMusteriler] = useState([]);
  const [musteriId, setMusteriId] = useState(preselectedMusteriId);
  const [baslangicBakiyesi, setBaslangicBakiyesi] = useState('0');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    getMusteriler()
      .then(setMusteriler)
      .catch(() => setError('Müşteri listesi yüklenemedi.'))
      .finally(() => setLoading(false));
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      const yeniHesap = await createHesap({
        musteriId: Number(musteriId),
        baslangicBakiyesi: Number(baslangicBakiyesi),
      });
      navigate(`/hesaplar/${yeniHesap.id}`);
    } catch (err) {
      const message = err.response?.data?.message || 'Hesap açılırken bir hata oluştu.';
      setError(message);
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <div className="state-message">Yükleniyor...</div>;

  return (
    <div className="hesap-form">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Hesap Yönetimi</div>
          <h1 className="page-header__title">Hesap Aç</h1>
        </div>
      </div>

      <form className="form-card" onSubmit={handleSubmit}>
        {error && <div className="state-message state-message--error form-error">{error}</div>}

        <div className="form-field">
          <label htmlFor="musteriId">Müşteri</label>
          <select
            id="musteriId"
            value={musteriId}
            onChange={(e) => setMusteriId(e.target.value)}
            required
          >
            <option value="" disabled>Müşteri seçin</option>
            {musteriler.map((m) => (
              <option key={m.id} value={m.id}>
                {m.ad} {m.soyad} — {m.tcKimlikNo}
              </option>
            ))}
          </select>
        </div>

        <div className="form-field">
          <label htmlFor="baslangicBakiyesi">Başlangıç Bakiyesi (₺)</label>
          <input
            id="baslangicBakiyesi"
            type="number"
            min="0"
            step="0.01"
            value={baslangicBakiyesi}
            onChange={(e) => setBaslangicBakiyesi(e.target.value)}
            required
          />
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn--primary" disabled={saving}>
            {saving ? 'Açılıyor...' : 'Hesabı Aç'}
          </button>
          <Link to="/hesaplar" className="btn btn--secondary">
            Vazgeç
          </Link>
        </div>
      </form>
    </div>
  );
}

export default HesapForm;