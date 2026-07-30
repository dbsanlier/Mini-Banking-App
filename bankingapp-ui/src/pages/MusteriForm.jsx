import { useEffect, useState } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { getMusteriById, createMusteri, updateMusteri } from '../services/musteriService';
import './MusteriForm.css';

const emptyForm = {
  ad: '',
  soyad: '',
  tcKimlikNo: '',
  telefon: '',
  email: '',
};

function MusteriForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState(emptyForm);
  const [loading, setLoading] = useState(isEdit);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!isEdit) return;

    getMusteriById(id)
      .then((data) =>
        setForm({
          ad: data.ad,
          soyad: data.soyad,
          tcKimlikNo: data.tcKimlikNo,
          telefon: data.telefon,
          email: data.email,
        })
      )
      .catch(() => setError('Müşteri bilgileri yüklenemedi.'))
      .finally(() => setLoading(false));
  }, [id, isEdit]);

  const handleChange = (field) => (e) => {
    setForm((prev) => ({ ...prev, [field]: e.target.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    try {
      if (isEdit) {
        // Guncellemede TC Kimlik No gonderilmiyor (backend'de de yok)
        const { tcKimlikNo, ...updateData } = form;
        await updateMusteri(id, updateData);
      } else {
        await createMusteri(form);
      }
      navigate('/');
    } catch (err) {
      const message = err.response?.data?.message || 'İşlem sırasında bir hata oluştu.';
      setError(message);
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <div className="state-message">Yükleniyor...</div>;
  }

  return (
    <div className="musteri-form">
      <div className="page-header">
        <div>
          <div className="page-header__eyebrow">Müşteri Yönetimi</div>
          <h1 className="page-header__title">{isEdit ? 'Müşteri Düzenle' : 'Yeni Müşteri'}</h1>
        </div>
      </div>

      <form className="form-card" onSubmit={handleSubmit}>
        {error && <div className="state-message state-message--error form-error">{error}</div>}

        <div className="form-field">
          <label htmlFor="ad">Ad</label>
          <input
            id="ad"
            type="text"
            value={form.ad}
            onChange={handleChange('ad')}
            required
          />
        </div>

        <div className="form-field">
          <label htmlFor="soyad">Soyad</label>
          <input
            id="soyad"
            type="text"
            value={form.soyad}
            onChange={handleChange('soyad')}
            required
          />
        </div>

        <div className="form-field">
          <label htmlFor="tcKimlikNo">TC Kimlik No</label>
          <input
            id="tcKimlikNo"
            type="text"
            value={form.tcKimlikNo}
            onChange={handleChange('tcKimlikNo')}
            required
            disabled={isEdit}
            maxLength={11}
          />
        </div>

        <div className="form-field">
          <label htmlFor="telefon">Telefon</label>
          <input
            id="telefon"
            type="tel"
            value={form.telefon}
            onChange={handleChange('telefon')}
            required
          />
        </div>

        <div className="form-field">
          <label htmlFor="email">E-posta</label>
          <input
            id="email"
            type="email"
            value={form.email}
            onChange={handleChange('email')}
            required
          />
        </div>

        <div className="form-actions">
          <button type="submit" className="btn btn--primary" disabled={saving}>
            {saving ? 'Kaydediliyor...' : isEdit ? 'Güncelle' : 'Oluştur'}
          </button>
          <Link to="/" className="btn btn--secondary">
            Vazgeç
          </Link>
        </div>
      </form>
    </div>
  );
}

export default MusteriForm;